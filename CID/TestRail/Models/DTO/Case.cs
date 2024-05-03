using Newtonsoft.Json;

namespace CID.TestRail.Models.DTO
{
    public class Case
    {
        /// <summary> Id of the case. /// </summary>
        public ulong Id { get; set; }

        /// <summary> Id of the Milestone related to the case. /// </summary>
        public ulong? MilestoneId { get; set; }

        /// <summary>Id of the template to be used./// </summary>
        public ulong TemplateId { get; set; }
        public ulong PriorityId { get; set; }
        public ulong SectionId { get; set; }
        public ulong SuiteId { get; set; }
        public ulong TypeId { get; set; }
        public ulong UpdatedBy { get; set; }

        [JsonProperty("created_by")]
        public ulong CreatorId { get; set; }

        public string Title { get; set; }
        public string Refs { get; set; }


        //implement a proper way to deserialize datetime
        //public DateTime? CreatedOn { get; set; }
        //public DateTime? UpdatedOn { get; set; }

        //public TimeSpan? Estimate { get; set; }
        //public TimeSpan? EstimateForecast { get; set; }
    }
}