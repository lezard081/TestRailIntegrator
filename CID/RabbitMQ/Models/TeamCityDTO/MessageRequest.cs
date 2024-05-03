using CID.RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.RabbitMQ.Models.TeamCityDTO
{
    public class MessageRequest
    {
        public string Sender { get; set; }

        public Message Message { get; set; }
    }
}
