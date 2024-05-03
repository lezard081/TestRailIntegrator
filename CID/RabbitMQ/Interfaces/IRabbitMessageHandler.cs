using CID.RabbitMQ.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;

namespace CID.RabbitMQ.Interfaces
{
    public interface IRabbitMessageHandler
    {
        List<Payload> Payloads { get; set; }
        void Handle(BasicGetResult result);
        void Handle(object model, BasicDeliverEventArgs result);
    }
}
