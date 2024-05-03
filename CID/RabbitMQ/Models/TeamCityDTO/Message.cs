using CID.RabbitMQ.Models.TestRailDTO;

namespace CID.RabbitMQ.Models.TeamCityDTO
{
    public class Message
    {
        /// <summary>
        /// Build ID from TeamCity
        /// </summary>
        public string BuildId { get; set; }

        /// <summary>
        /// A Test Suite
        /// </summary>
        public Suites Suite { get; set; }
    }
}
