using CID.RabbitMQ.Utils;

namespace CID.RabbitMQ.Interfaces
{
    public interface IRabbitPublisherSetup
    {
        IRabbitPublisher Setup(RabbitConfiguration rabbitConfiguration);
    }
}
