using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public static bool ToBeUpdated(DateTime start, DateTime end, List<LeadActivity> leadsActivitiesList)
        {
            return
                leadsActivitiesList.Count == 0
                || leadsActivitiesList.Max(la => la.ActivityDate) <= end.AddHours(-12)
                || leadsActivitiesList.Min(la => la.ActivityDate) >= start.AddHours(12);
        }
    }
}
