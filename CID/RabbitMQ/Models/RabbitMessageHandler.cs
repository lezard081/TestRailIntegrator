using CID.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ.Models
{
    public class RabbitMessageHandler : IRabbitMessageHandler
    {
        public List<Payload> Payloads { get; set; }

        public void Handle(BasicGetResult result)
        {
            Payloads.Add(new Payload()
            {
                Body = Encoding.UTF8.GetString(result.Body.ToArray()),
                MessageId = result.BasicProperties.MessageId,
                CorrelationId = result.BasicProperties.CorrelationId,
                ReplyTo = result.BasicProperties.ReplyTo
            });
        }

        public void Handle(object model, BasicDeliverEventArgs result)
        {
            Payloads.Add(new Payload()
            {
                Body = Encoding.UTF8.GetString(result.Body.ToArray()),
                MessageId = result.BasicProperties.MessageId,
                CorrelationId = result.BasicProperties.CorrelationId,
                ReplyTo = result.BasicProperties.ReplyTo
            });
        }
    }
}
