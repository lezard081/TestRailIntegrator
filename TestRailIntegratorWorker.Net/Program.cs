using System;
using CID.RabbitMQ;
using System.Configuration;
using RabbitMQ.Client;
using CID.RabbitMQ.Models;
using Microsoft.Extensions.Logging;
using System.ServiceProcess;
using CID.Logger;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Hosting;
using TestRailIntegratorWorker.Models;

namespace TestRailIntegratorWorkerService
{
    public class Program
    {
        private static volatile bool _cancelled = false;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs args)
            {
                args.Cancel = true;
                Program._cancelled = true;
            };

            var configurationBuilder = new ConfigurationBuilder();
            BuildConfig(configurationBuilder);

            IConfiguration config = configurationBuilder.Build();

            WorkerConfiguration workerConfig = config.GetSection("Worker").Get<WorkerConfiguration>();

            while (true)
            {
                Start(workerConfig);
            }
        }

        private static void Start(WorkerConfiguration workerConfig)
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

            logger.LogInformation("Settings initialized successfully.");

           
            logger.LogInformation("Starting TestRailWorker...");

            RabbitMQService<TestRailConsumer, DefaultRabbitPublisher> rabbitMQService = new RabbitMQService<TestRailConsumer, DefaultRabbitPublisher>(GetConnectionFactory(workerConfig));
            try
            {
                Guid consumerID = rabbitMQService.Subscribe(workerConfig.Exchange, workerConfig.Queue, new TestRailMessageHandler(), workerConfig.RoutingKey, workerConfig.Type, workerConfig.Durable);
                logger.LogInformation("Consumer {0} created, waiting for messages.", consumerID);

                Console.ReadLine();

                rabbitMQService.Unsubscribe(consumerID);
            }
            catch (Exception e)
            {
                logger.LogError("[ERROR] [{0}] {1}\n [InnerException]: \n {2}", e.Source, e.Message, e.InnerException);
            }
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private static ConnectionFactory GetConnectionFactory(WorkerConfiguration workerConfig)
        {
            return new ConnectionFactory()
            {
                HostName = workerConfig.HostName,
                Port = workerConfig.Port,
                UserName = workerConfig.UserName,
                Password = workerConfig.Password,
                VirtualHost = workerConfig.VirtualHost, 
                RequestedHeartbeat = TimeSpan.FromSeconds(30),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }
        
    }
}
