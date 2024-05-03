using CID.RabbitMQ.Interfaces;
using CID.RabbitMQ.Extensions;
using CID.RabbitMQ.Models.TestRailDTO;
using CID.RabbitMQ.Models.TeamCityDTO;
using CID.TestRail;
using CID.TestRail.Enums;
using CID.TestRail.Models.DTO;
using CID.TestRail.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ.Models
{
    public class TestRailMessageHandler : IRabbitMessageHandler
    {
        public List<Payload> Payloads { get; set; }


        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };

        public void Handle(BasicGetResult result)
        {
            string body = Encoding.UTF8.GetString(result.Body.ToArray());

            List<RequestResult<Result>> results = UpdateTestRailTestCaseResults(body);

            Payloads.Add(new Payload()
            {
                Body = JsonConvert.SerializeObject(results),
                MessageId = result.BasicProperties.MessageId,
                CorrelationId = result.BasicProperties.CorrelationId,
                ReplyTo = result.BasicProperties.ReplyTo
            });
        }

        public void Handle(object model, BasicDeliverEventArgs eventArgs)
        {
            string body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            Console.WriteLine("Message Received: {0}", body);

            if (body.Equals("[]"))
                throw new ArgumentNullException("Empty message found!");

            List<RequestResult<Result>> results = UpdateTestRailTestCaseResults(body);

            Payloads = new List<Payload>();

            string newBody = JsonConvert.SerializeObject(results, _jsonSettings);

            Payloads.Add(new Payload()
            {
                Body = newBody,
                MessageId = eventArgs.BasicProperties.MessageId,
                CorrelationId = eventArgs.BasicProperties.CorrelationId,
                ReplyTo = eventArgs.BasicProperties.ReplyTo
            });
        }

        #region private methods

        private static List<RequestResult<Result>> UpdateTestRailTestCaseResults(string body)
        {
            ulong currentRunId = 0;

            List<RequestResult<Result>> results = new List<RequestResult<Result>>();

            try
            {
                MessageRequest messageRequestFromMq = JsonConvert.DeserializeObject<MessageRequest>(body, _jsonSettings);

                TestRailClient testRailClient = new TestRailClient();

                Task<RequestResult<List<Run>>> runs = testRailClient.GetRunsFromSuite(messageRequestFromMq.Message.Suite.ProjectId, messageRequestFromMq.Message.Suite.Id, options: $"&is_completed=0");
                runs.Wait();

                foreach (Run run in runs.Result.Content)
                {
                    if (run.Name.Contains(messageRequestFromMq.Message.BuildId))
                    {
                        currentRunId = run.Id;
                        break;
                    }
                }

                if (currentRunId == 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine($"Automated Tests Run on TeamCity");
                    stringBuilder.AppendLine($"Build: {messageRequestFromMq.Message.BuildId}");

                    Task<RequestResult<Run>> newRun = testRailClient.AddRun(messageRequestFromMq.Message.Suite.ProjectId, $"Automated Tests Build Number: {messageRequestFromMq.Message.BuildId}", stringBuilder.ToString(), suiteId: messageRequestFromMq.Message.Suite.Id);
                    newRun.Wait();

                    currentRunId = newRun.Result.Content.Id;
                }

                Task<RequestResult<List<Test>>> tests = testRailClient.GetTestsFromRun(currentRunId, $"&status_id={(int)ResultStatus.Untested}");

                //check if the TestCase to be updated in TestRail is untested in the current TestRun in TestRail.
                foreach (TestCase teamCityBuild in messageRequestFromMq.Message.Suite.Test.Cases)
                {
                    if (tests.Result.Content.Exists(testRailTestCase => testRailTestCase.Title.Equals(teamCityBuild.Name)))
                    {
                        switch (teamCityBuild.Result.ToUpper().ParseToTestRailStatus())
                        {
                            case ResultStatus.Passed:
                                var result = Task.Run(() => testRailClient.AddResult(tests.Result.Content.Find(tc => tc.Title.Equals(teamCityBuild.Name)).Id.Value, ResultStatus.Passed, null, string.Format("Automated Test Result: {0}", ResultStatus.Passed.ToString())));
                                result.Wait();

                                results.Add(result.Result);
                                break;

                            case ResultStatus.Failed:
                                StringBuilder errorMessage = new StringBuilder();
                                errorMessage.AppendLine(string.Format("Test Failed please see build logs in: {0}.", teamCityBuild.Logs));
                                errorMessage.AppendLine();

                                result = Task.Run(() => testRailClient.AddResult(tests.Result.Content.Find(testRailTestCase => testRailTestCase.Title.Equals(teamCityBuild.Name)).Id.Value, ResultStatus.Failed, null, errorMessage.ToString()));
                                result.Wait();

                                results.Add(result.Result);
                                break;

                        }

                        tests.Result.Content.RemoveAt(tests.Result.Content.FindIndex(testRailTestCase => testRailTestCase.Title.Equals(teamCityBuild.Name)));
                    }
                }

                if (tests.Result.Content.Count == 0)
                {
                    var closeRun = Task.Run(() => testRailClient.CloseRun(currentRunId));
                    closeRun.Wait();
                }

                return results;

            }
            catch (JsonSerializationException jse)
            {
                throw new JsonSerializationException(jse.Message, jse.Path, jse.LineNumber, jse.LinePosition, jse.InnerException);
            }
            catch (NullReferenceException nre)
            {
                throw new NullReferenceException(nre.Message, nre.InnerException);
            }
        }
        #endregion private methods
    }
}
