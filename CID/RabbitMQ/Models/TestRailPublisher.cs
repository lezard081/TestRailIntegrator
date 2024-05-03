using System.Collections.Generic;
using System.Text;
using CID.RabbitMQ.Interfaces;
using CID.RabbitMQ.Utils;
using RabbitMQ.Client;

namespace CID.RabbitMQ.Models
{
    public class TestRailPublisher : IRabbitPublisher
    {
        private RabbitConfiguration _rabbitConfiguration;

        public IRabbitPublisher Setup(RabbitConfiguration rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
            return this;
        }

        public void Publish(IEnumerable<Payload> payloads)
        {
            using (IConnection connection = _rabbitConfiguration.ConnectionFactory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(_rabbitConfiguration.Exchange, _rabbitConfiguration.Type, _rabbitConfiguration.Durable, false, null);
                channel.QueueBind(_rabbitConfiguration.Queue, _rabbitConfiguration.Exchange, _rabbitConfiguration.RoutingKey);

                foreach (Payload payload in payloads)
                {
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.CorrelationId = payload.CorrelationId ?? "";
                    properties.ReplyTo = payload.ReplyTo ?? "";

                    byte[] body = Encoding.UTF8.GetBytes(payload.Body);

                    channel.BasicPublish(_rabbitConfiguration.Exchange, _rabbitConfiguration.RoutingKey, true, properties, body);
                }
            }
        }
    }
}
