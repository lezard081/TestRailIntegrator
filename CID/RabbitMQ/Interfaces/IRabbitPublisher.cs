using CID.RabbitMQ.Models;
using System.Collections.Generic;

namespace CID.RabbitMQ.Interfaces
{
    public interface IRabbitPublisher : IRabbitPublisherSetup
    {
        void Publish(IEnumerable<Payload> payloads);
    }
}
