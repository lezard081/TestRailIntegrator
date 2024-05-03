using CID.TestRail.Enums;
using System;

namespace CID.TestRail.Models.DTO
{
    public class Result
    {
        /// <summary>ID of the result.</summary>
        public ulong Id { get; set; }

        /// <summary>ID of the test.</summary>
        public ulong TestId { get; set; }

        /// <summary>Case ID of the test.</summary>
        public ulong CaseId { get; set; }

        /// <summary>ID of the test status.</summary>
        public ResultStatus? StatusId { get; set; }

        /// <summary>Created by.</summary>
        public ulong? CreatedBy { get; set; }

        /// <summary>Result created on.</summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>The ID of the user the test should be assigned to.</summary>
        public ulong? AssignedToId { get; set; }

        /// <summary>The comment/description for the test result.</summary>
        public string Comment { get; set; }

        /// <summary>The version or build tested against.</summary>
        public string Version { get; set; }

        /// <summary>The time it took to execute the test.</summary>
        public TimeSpan? Elapsed { get; set; }

        /// <summary>A comma-separated list of defects to link to the test result.</summary>
        public string Defects { get; set; }
    }
}
