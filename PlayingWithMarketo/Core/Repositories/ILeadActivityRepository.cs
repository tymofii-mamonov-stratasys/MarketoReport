using PlayingWithMarketo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayingWithMarketo.Core.Repositories
{
    public interface ILeadActivityRepository
    {
        List<LeadActivity> GetLeadActivities(DateTime startDate, DateTime endDate);
        IQueryable<LeadActivity> GetLeadActivitiesWithIncludes(DateTime startDate, DateTime endDate);
        void AddLeadActivity(LeadActivity leadActivity);
    }
}
