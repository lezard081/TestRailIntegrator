using CID.RabbitMQ.Interfaces;
using CID.RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ
{
    public interface IRabbitMQService<TConsumer, TPublisher> where TConsumer : class, IRabbitConsumerSetup, new() where TPublisher : class, IRabbitPublisherSetup, new()
    {
        void Publish(IEnumerable<Payload> payloads, string exchange, string routingKey = "", string type = "fanout", bool durable = false);
        void Get(string exchange, string queue, IRabbitMessageHandler messageHandler, string routingKey = "", string type = "fanout", bool durable = false);
        Guid Subscribe(string exchange, string queue, IRabbitMessageHandler messageHandler, string routingKey = "", string type = "fanout", bool durable = false);
        void Unsubscribe(Guid consumerId);
    }
}
