using CID.RabbitMQ.Interfaces;
using CID.RabbitMQ.Models;
using CID.RabbitMQ.Utils;
using CID.TestRail;
using CID.TestRail.Enums;
using CID.TestRail.Models;
using CID.TestRail.Utils;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ.Models
{
    public class TestRailConsumer : IRabbitConsumer
    {
        private RabbitConfiguration _rabbitConfiguration;
        private EventingBasicConsumer _consumer;
        private IRabbitPublisherSetup _rabbitPublisher;

        private IConnection _connection;
        private IModel _channel;

        private string _consumerTag;

        public void Consume(IRabbitMessageHandler messageHandler)
        {
            if (_rabbitConfiguration == null)
                throw new ApplicationException("Rabbit configuration is missing.");

            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            _consumer = new EventingBasicConsumer(_channel);
            _rabbitPublisher = new TestRailPublisher();

            _consumer.Received += (model, eventArgs) =>
            {
                try
                {
                    messageHandler.Handle(model, eventArgs);

                    foreach (Payload payload in messageHandler.Payloads)
                    {
                        PublishToLogs(messageHandler, payload);
                    }

                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception e)
                {
                    _channel.BasicReject(eventArgs.DeliveryTag, false);
                }
            };

            _consumerTag = _channel.BasicConsume(_rabbitConfiguration.Queue, false, _consumer);
        }

        private void PublishToLogs(IRabbitMessageHandler messageHandler, Payload payload)
        {
            RabbitConfiguration config = new RabbitConfiguration(_rabbitConfiguration.ConnectionFactory, "testrail.log.exchange", "testrail.log.*", ExchangeType.Topic, false, payload.ReplyTo);

            _rabbitPublisher.Setup(config).Publish(messageHandler.Payloads);
        }

        public void Dispose()
        {
            if (_channel?.IsOpen == true && _consumer != null)
            {
                _channel.BasicCancel(_consumerTag);
            }

            _channel?.Dispose();
            _connection?.Dispose();
        }

        public IRabbitConsumer Setup(RabbitConfiguration rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
            _connection = _rabbitConfiguration.ConnectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_rabbitConfiguration.Exchange, _rabbitConfiguration.Type, _rabbitConfiguration.Durable, false);
            _channel.QueueDeclare(_rabbitConfiguration.Queue, _rabbitConfiguration.Durable, false, false, null);
            _channel.QueueBind(_rabbitConfiguration.Queue, _rabbitConfiguration.Exchange, _rabbitConfiguration.RoutingKey);

            return this;
        }

        #region unused methods
        public void Get(IRabbitMessageHandler messageHandler)
        {
            throw new NotImplementedException();
        }
        #endregion unsued methods
    }
}
