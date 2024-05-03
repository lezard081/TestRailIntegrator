using CID.RabbitMQ.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ.Interfaces
{
    public interface IRabbitConsumer : IRabbitConsumerSetup
    {
        void Get(IRabbitMessageHandler messageHandler);
        void Consume(IRabbitMessageHandler messageHandler);
    }
}
