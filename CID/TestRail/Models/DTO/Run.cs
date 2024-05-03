using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CID.TestRail.Models.DTO
{
    public class Run
    {
        public ulong Id { get; set; }
        public ulong? SuiteId { get; set; }
        public ulong? MilestoneId { get; set; }
        public ulong? PassedCount { get; set; }
        public ulong? BlockedCount { get; set; }
        public ulong? UntestedCount { get; set; }
        public ulong? RetestCount { get; set; }



        [JsonProperty("assignedto_id")]
        public ulong? AssigneeToId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public Boolean IncludeAll { get; set; }
        public HashSet<ulong> CaseIds { get; set; }

        public ulong CustomStatus1Count { get; set; }
        public ulong CustomStatus2Count { get; set; }
        public ulong CustomStatus3Count { get; set; }
        public ulong CustomStatus4Count { get; set; }
        public ulong CustomStatus5Count { get; set; }
        public ulong CustomStatus6Count { get; set; }
        public ulong CustomStatus7Count { get; set; }

        public List<string> Refs { get; set; }
    }
}