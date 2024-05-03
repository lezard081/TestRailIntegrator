using CID.RabbitMQ.Interfaces;
using CID.RabbitMQ.Models;
using CID.RabbitMQ.Utils;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace CID.RabbitMQ
{
    public class RabbitMQService<TConsumer, TPublisher> : IRabbitMQService<TConsumer, TPublisher> where TConsumer : class, IRabbitConsumerSetup, new() where TPublisher : class, IRabbitPublisherSetup, new()
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ConcurrentDictionary<Guid, TConsumer> _consumers = new ConcurrentDictionary<Guid, TConsumer>();

        public RabbitMQService(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Publish(IEnumerable<Payload> payloads, string exchange, string routingKey = "", string type = "fanout", bool durable = false)
        {
            RabbitConfiguration rabbitConfiguration = new RabbitConfiguration(_connectionFactory, exchange, routingKey, type, durable);

            TPublisher messagePublisher = new TPublisher();

            messagePublisher.Setup(rabbitConfiguration).Publish(payloads);
        }

        public void Get(string exchange, string queue, IRabbitMessageHandler messageHandler, string routingKey = "", string type = "fanout", bool durable = false)
        {
            RabbitConfiguration rabbitConfiguration = new RabbitConfiguration(_connectionFactory, exchange, routingKey, type, durable, queue);

            using (var messageConsumer = new TConsumer())
            {
                messageConsumer.Setup(rabbitConfiguration).Get(messageHandler);
            }
        }

        public Guid Subscribe(string exchange, string queue, IRabbitMessageHandler messageHandler, string routingKey = "", string type = "fanout", bool durable = false)
        {
            RabbitConfiguration rabbitConfiguration = new RabbitConfiguration(_connectionFactory, exchange, routingKey, type, durable, queue);

            TConsumer consumer = new TConsumer();
            Guid consumerId = Guid.NewGuid();

            _consumers.TryAdd(consumerId, consumer);
            consumer.Setup(rabbitConfiguration).Consume(messageHandler);

            return consumerId;
        }

        public void Unsubscribe(Guid consumerId)
        {
            var consumer = _consumers[consumerId];
            consumer.Dispose();
            _consumers.TryRemove(consumerId, out TConsumer _);
        }

    }
}
