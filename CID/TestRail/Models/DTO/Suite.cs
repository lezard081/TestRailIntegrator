using System;

namespace CID.TestRail.Models.DTO
{
    public class Suite
    {
        /// <summary>ID of the Suite.</summary>
        public int Id { get; set; }

        /// <summary>ID of the Project.</summary>
        public int ProjectId { get; set; }

        /// <summary>Name of the Suite.</summary>
        public string Name { get; set; }

        /// <summary>Description of the Suite</summary>
        public string Description { get; set; }

        /// <summary>Url to view the suite.</summary>
        public string Url { get; set; }

        /// <summary>Flag if Suite is Baseline</summary>
        public Boolean IsBaseline { get; set; }

        /// <summary>Flag if Suite is Master.</summary>
        public Boolean IsMaster { get; set; }

        /// <summary>Flag if Suite is completed/archived.</summary>
        public Boolean IsCompleted { get; set; }

        /// <summary>Date when suite was closed. </summary>
        public DateTime? CompletedOn { get; set; }

    }
}