using CID.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace CID.RabbitMQ.Utils
{
    public class RabbitConfiguration : IRabbitConfiguration
    {
        public ConnectionFactory ConnectionFactory { get; }

        public string Exchange { get; }
        public string Queue { get; }
        public string RoutingKey { get; }
        public string Type { get; }

        public bool Durable { get; }

        public RabbitConfiguration(ConnectionFactory connectionFactory, string exchange, string routingKey, string type, bool durable, string queue = "")
        {
            ConnectionFactory = connectionFactory;
            Exchange = exchange;
            Queue = queue;
            RoutingKey = routingKey;
            Type = type;
            Durable = durable;
        }
    }
}
