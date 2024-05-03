namespace CID.TestRail.Models.DTO
{
    public class BulkResults<T>
    {
        public ulong Offset { get; set; }
        public ulong Limit { get; set; }
        public ulong Size { get; set; }
        public Links _Links { get; set; }

        public class Links
        {
            public string Next { get; set; }
            public string Prev { get; set; }
        }
    }
}