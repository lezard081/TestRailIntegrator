using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ.Interfaces
{
    public interface IRabbitConfiguration
    {
        ConnectionFactory ConnectionFactory { get; }
        string Exchange { get; }
        string Queue { get; }
        string RoutingKey { get; }
        string Type { get; }

        bool Durable { get; }
    }
}
