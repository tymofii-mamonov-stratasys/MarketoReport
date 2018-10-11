using System;

namespace PlayingWithMarketo.Marketo.DTO
{
    public class Result
    {
        public string exportId { get; set; }
        public string format { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime queuedAt { get; set; }
        public DateTime startedAt { get; set; }
        public DateTime finishedAt { get; set; }
        public int numberOfRecords { get; set; }
        public int fileSize { get; set; }
        public string sfdcLeadId { get; set; }
        public string SFDCCampaignID { get; set; }
    }
}
