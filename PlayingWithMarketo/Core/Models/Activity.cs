using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayingWithMarketo.Core.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [Required]
        public int ActivityId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ActivityName { get; set; }
        public ICollection<LeadActivity> LeadActivities { get; set; }
    }
}
