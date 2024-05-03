using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ.Models
{
    public class Payload
    {
        public string Label { get; set; }
        public string MessageId { get; set; }
        public string CorrelationId { get; set; }
        public string ReplyTo { get; set; }
        public string Body { get; set; }
    }
}
