using CID.TestRail.Enums;

namespace CID.TestRail.Models.DTO
{
    public class Test
    {
        /// <summary>ID of the test.</summary>
        public ulong? Id { get; set; }

        /// <summary>ID of the test case.</summary>
        public ulong? CaseId { get; set; }

        /// <summary>ID of the test run.</summary>
        public ulong? RunId { get; set; }

        /// <summary>TestRail status ID.</summary>
        public ResultStatus? Status { get; set; }

        /// <summary>ID of the user the test is assigned to.</summary>
        public ulong? AssignedToId { get; set; }

        /// <summary>Title of the test.</summary>
        public string Title { get; set; }
    }
}
