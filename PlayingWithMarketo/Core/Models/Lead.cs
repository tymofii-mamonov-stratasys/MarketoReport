using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayingWithMarketo.Core.Models
{
    public class Lead
    {
        public int Id { get; set; }
        [Required]
        public int LeadId { get; set; }

        [MaxLength(250)]
        public string CampaignId { get; set; }

        [MaxLength(25)]
        public string SFDCId { get; set; }
        public ICollection<LeadActivity> LeadActivities { get; set; }
    }
}