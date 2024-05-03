using CID.Logger;
using CID.RabbitMQ;
using CID.RabbitMQ.Models;
using Microsoft.Extensions.Logging;
using System;
using TestRailIntegratorWorker.Models;

namespace TestRailIntegratorWorker
{
    public class RunService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly WorkerConfiguration _workerConfiguration;

        public RunService(WorkerConfiguration workerConfiguration, ILoggerFactory loggerFactory)
        {
            _workerConfiguration = workerConfiguration;
            _loggerFactory = loggerFactory;
        }
        public void Run()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                       .AddFilter("Microsoft", LogLevel.Warning)
                       .AddFilter("System", LogLevel.Warning)
                       .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                       .AddConsole();
            });

            var logger = new LoggerWrapper(loggerFactory.CreateLogger<LoggerWrapper>());

            logger.LogInformation("Initializing Settings for {0}", _workerConfiguration.WorkerName);


            logger.LogInformation("Settings initialized successfully.");

            //_workerConfiguration.Run();


            //switch (_workerConfiguration.WorkerName)
            //{
            //    case "TestRailWorker":
            //        logger.LogInformation("Starting TestRailWorker...");

            //        RabbitMQService<TestRailConsumer, DefaultRabbitPublisher> rabbitMQService = new RabbitMQService<TestRailConsumer, DefaultRabbitPublisher>(_connectionFactory);
            //        try
            //        {
            //            Guid consumerID = rabbitMQService.Subscribe(_workerConfiguration.Exchange, _workerConfiguration.Queue, new TestRailMessageHandler(), _workerConfiguration.RoutingKey, _workerConfiguration.Type, _workerConfiguration.Durable);
            //            logger.LogInformation("Consumer {0} created, waiting for messages.", consumerID);

            //            Console.ReadLine();

            //            rabbitMQService.Unsubscribe(consumerID);
            //        }
            //        catch (Exception e)
            //        {
            //            logger.LogError("[ERROR] [{0}] {1}\n [InnerException]: \n {2}", e.Source, e.Message, e.InnerException);
            //        }

            //        break;
            //    default:
            //        logger.LogError("Unable to find worker with the name {0}.", _workerConfiguration.WorkerName);
            //        break;
            //}
        }
    }
}
