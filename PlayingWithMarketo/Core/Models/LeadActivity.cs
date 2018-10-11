using System;
using System.ComponentModel.DataAnnotations;

namespace PlayingWithMarketo.Core.Models
{
    public class LeadActivity
    {
        public int Id { get; set; }
        public Lead Lead { get; set; }
        public int LeadId { get; set; }
        public DateTime ActivityDate { get; set; }
        public Activity Activity { get; set; }
        public int ActivityId { get; set; }
        [MaxLength(4000)]
        public string Attributes { get; set; }
    }
}
