using System;
using System.ComponentModel.DataAnnotations;

namespace PlayingWithMarketo.Core.Models
{
    public class ExportJob
    {
        public int Id { get; set; }

        [Required]
        public string ExportId { get; set; }

        public string Status { get; set; }

        [Required]
        [MaxLength(3)]
        public string Format { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? QueuedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int? NumberOfRecords { get; set; }
        public int? FileSize { get; set; }
    }
}
