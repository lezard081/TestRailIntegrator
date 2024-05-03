using System;
using System.Collections.Generic;
using System.Text;

namespace TestRailIntegratorWorker.Models
{
    public class WorkerConfiguration
    {
        public string WorkerName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
        public string Queue { get; set; }
        public string VirtualHost { get; set; }
    }
}
