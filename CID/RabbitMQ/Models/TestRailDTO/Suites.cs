using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CID.RabbitMQ.Models.TestRailDTO
{
    public class Suites
    {
        public ulong Id { get; set; }

        public ulong ProjectId { get; set; }
        public Tests Test { get; set; }
    }
}