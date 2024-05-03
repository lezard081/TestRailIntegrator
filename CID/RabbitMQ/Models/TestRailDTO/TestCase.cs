using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CID.RabbitMQ.Models.TestRailDTO
{
    public class TestCase
    {
        public string Name { get; set; }
        public string Result { get; set; }
        public string Logs { get; set; }
    }
}