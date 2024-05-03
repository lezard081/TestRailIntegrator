using CID.TestRail.Enums;
using CID.TestRail.Models.DTO;
using CID.HttpHelper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CID.HttpHelper.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using CID.TestRail.Utils;

namespace CID.TestRail
{
    public class TestRailClient
    {
        private static HttpHelperClient _httpHelper;
        private static JsonSerializerSettings _jsonSettings;
        private Dictionary<string, string> _headers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseUrl">Base URL of TestRail Instance</param>
        public TestRailClient()
        {
            _jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };

            _headers = new Dictionary<string, string>();

            _httpHelper = new HttpHelperClient();
        }

        protected static string CreateUri(CommandType commandType, CommandAction actionName, ulong? id1 = null,
            ulong? id2 = null, string options = null, string id2Str = null)
        {
            string commandString = commandType.ToString();
            string actionString = actionName.ToString();

            string uri = $"?/api/v2/{commandString}_{actionString}{(id1.HasValue ? "/" + id1.Value : string.Empty)}{(id2.HasValue ? "/" + id2.Value : !string.IsNullOrWhiteSpace(id2Str) ? "/" + id2Str : string.Empty)}{(!string.IsNullOrWhiteSpace(options) ? options : string.Empty)}";

            return uri;
        }

        /// <summary>
        /// Get Run from TestRail.
        /// </summary>
        /// <param name="runId">The run ID of the Test Run to retrieve.</param>
        /// <returns>A Request Result with the Run Object.</returns>
        async public Task<RequestResult<Run>> GetRun(ulong runId)
        {
            string uri = CreateUri(CommandType.Get, CommandAction.Run, runId);

            return await RequestGet<Run>(uri);
        }

        /// <summary>
        /// Get runs under the specified project from TestRail.
        /// </summary>
        /// <param name="projectId">The ID of the project where to get the TestRuns.</param>
        /// <param name="options">Additional options used to filter results from TestRail.</param>
        /// <returns>A list of TestRuns under the specified project based on the submitted criteria.</returns>
        async public Task<RequestResult<List<Run>>> GetRuns(ulong projectId, string options = null)
        {
            string uri = CreateUri(CommandType.Get, CommandAction.Runs, projectId, options: options);

            return await RequestGet<List<Run>>(uri);
        }

        /// <summary>
        /// Get Test Runs that is taken from the specified suite.
        /// </summary>
        /// <param name="projectId">The ID of the project where to get the TestRuns.</param>
        /// <param name="suiteId">The ID of the suite where the TestRun is based from.</param>
        /// <param name="options">Additional options used to filter results from TestRail.</param>
        /// <returns>A list of TestRuns under the specified project based on the submitted criteria.</returns>
        async public Task<RequestResult<List<Run>>> GetRunsFromSuite(ulong projectId, ulong suiteId, string options = null)
        {
            string uri = CreateUri(CommandType.Get, CommandAction.Runs, projectId, suiteId, options);

            return await RequestGet<List<Run>>(uri);
        }

        /// <summary>
        /// Get Cases that are included in the Project Suite.
        /// </summary>
        /// <param name="projectId">The ID of the project where to get the Cases.</param>
        /// <param name="suiteId">The ID of the suite where to get the TestCases.</param>
        /// <returns>The list of the Test Cases in the Suite with their Case Id.</returns>
        async public Task<RequestResult<List<Case>>> GetCasesFromProjectSuite(ulong projectId, ulong suiteId)
        {
            string uri = CreateUri(CommandType.Get, CommandAction.Cases, projectId, options: $"&suite_id={suiteId}");

            return await RequestGet<List<Case>>(uri);
        }

        /// <summary>
        /// Get TestCases from the specified Run.
        /// </summary>
        /// <param name="runId">The id of the Run where to get the Test Cases.</param>
        /// <param name="options">Additional </param>
        /// <returns></returns>
        async public Task<RequestResult<List<Test>>> GetTestsFromRun(ulong runId, string options)
        {
            string uri = CreateUri(CommandType.Get, CommandAction.Tests, runId, options: options);

            return await RequestGet<List<Test>>(uri);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="suiteId"></param>
        /// <param name="milestoneId"></param>
        /// <param name="assignedToId"></param>
        /// <param name="caseIds"></param>
        /// <param name="includeAll"></param>
        /// <returns></returns>
        async public Task<RequestResult<Run>> AddRun(ulong projectId, string name, string description, ulong? suiteId = null, ulong? milestoneId = null, ulong? assignedToId = null, HashSet<ulong> caseIds = null, bool includeAll = true)
        {
            string uri = CreateUri(CommandType.Add, CommandAction.Run, projectId);

            Run run = new Run()
            {
                Name = name,
                Description = description,
                CaseIds = caseIds,
                SuiteId = suiteId,
                MilestoneId = milestoneId,
                AssigneeToId = assignedToId,
                IncludeAll = includeAll
            };

            return await SendPost<Run>(uri, run);
        }

        // <summary>
        /// Adds a new test result, comment or assigns a test.
        /// </summary>
        /// <param name="testId">The ID of the test the result should be added to.</param>
        /// <param name="status">The test status.</param>
        /// <param name="comment">The comment/description for the test result.</param>
        /// <param name="version">The version or build you tested against.</param>
        /// <param name="elapsed">The time it took to execute the test, e.g. "30s" or "1m 45s".</param>
        /// <param name="defects">A comma-separated list of defects to link to the test result.</param>
        /// <param name="assignedToId">The ID of a user the test should be assigned to.</param>
        /// <returns>If successful, this method will return the new test result.</returns>
        async public Task<RequestResult<Result>> AddResult(ulong caseId, ResultStatus? status, ulong? assignedToId, string comment = null, string version = null, TimeSpan? elapsed = null)
        {
            string uri = CreateUri(CommandType.Add, CommandAction.Result, caseId);

            Result result = new Result()
            {
                StatusId = status,
                Comment = comment,
                Version = version,
                Elapsed = elapsed,
                AssignedToId = assignedToId
            };

            return await SendPost<Result>(uri, result);
        }

        async public Task<RequestResult<Run>> CloseRun(ulong runId)
        {
            string uri = CreateUri(CommandType.Close, CommandAction.Run, runId);

            return await SendPost<Run>(uri, null);
        }

        public string GetBaseAddress()
        {
            return _httpHelper.GetBaseAddress();
        }

        #region private methods

        /// <summary>
        /// Method for GetRequests
        /// </summary>
        /// <typeparam name="T">Obect type to deserialize results to.</typeparam>
        /// <param name="uri">Uri path of REST API in TestRail.</param>
        /// <returns>A RequestResult<T> object that holds the response from the request.</returns>
        private async Task<RequestResult<T>> RequestGet<T>(string uri)
        {
            try
            {
                HttpResponse response = await _httpHelper.MakeHttpGetRequest(uri, clientName: ClientType.TestRailGet.ToString());

                return new RequestResult<T>(response.StatusCode, response.Content);
            }

            catch (Exception exception)
            {
                return ProcessException<T>(exception);
            }
        }

        /// <summary>
        /// Method to Send Post Requests
        /// </summary>
        /// <typeparam name="T">Object type to deserialize results to.</typeparam>
        /// <param name="uri">Uri path of REST API in TestRail.</param>
        /// <param name="content">Content to send to TestRail.</param>
        /// <returns>A RequestResult<T> object that holds the response from the request.</returns>
        private async Task<RequestResult<T>> SendPost<T>(string uri, T content)
        {
            string jsonContent = String.Empty;

            if (content != null)
            {
                jsonContent = JsonConvert.SerializeObject(content, _jsonSettings);
            }

            try
            {
                HttpResponse response = await _httpHelper.MakeHttpPostRequest(uri, null, jsonContent, ClientType.TestRailPost.ToString());

                return new RequestResult<T>(response.StatusCode, response.Content);
            }
            catch (Exception exception)
            {
                return ProcessException<T>(exception);
            }
        }

        /// <summary>
        /// Processes Exception thrown by HttpRequests
        /// </summary>
        /// <typeparam name="T">The object type to deserialize the response to.</typeparam>
        /// <param name="exception">The exception thrown to be processed to a RequestResult<T> object</param>
        /// <returns>A RequestResult<T> object which contains the exception.</returns>
        private static RequestResult<T> ProcessException<T>(Exception exception)
        {
            string message = exception.Message;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (message.Contains("400"))
                statusCode = HttpStatusCode.BadRequest;

            if (message.Contains("401"))
                statusCode = HttpStatusCode.Unauthorized;

            if (message.Contains("403"))
                statusCode = HttpStatusCode.Forbidden;

            if (message.Contains("404"))
                statusCode = HttpStatusCode.NotFound;

            if (message.Contains("502"))
                statusCode = HttpStatusCode.BadGateway;

            if (message.Contains("503"))
                statusCode = HttpStatusCode.ServiceUnavailable;

            if (message.Contains("504"))
                statusCode = HttpStatusCode.GatewayTimeout;

            return new RequestResult<T>(statusCode, thrownException: exception);
        }

        #endregion private methods

    }
}
