using CID.RabbitMQ.Utils;
using System;

namespace CID.RabbitMQ.Interfaces
{
    public interface IRabbitConsumerSetup : IDisposable
    {
        IRabbitConsumer Setup(RabbitConfiguration rabbitConfiguration);
    }
}
