using PlayingWithMarketo.Core.Models;
using PlayingWithMarketo.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PlayingWithMarketo.Persistance.Repositories
{
    public class LeadActivityRepository : ILeadActivityRepository
    {
        private readonly IMarketoDbContext _context;
        public List<LeadActivity> GetLeadActivities(DateTime startDate, DateTime endDate)
        {
            return _context.LeadActivities
                    .Where(a => a.ActivityDate >= startDate && a.ActivityDate <= endDate)
                    .ToList();
        }

        public IQueryable<LeadActivity> GetLeadActivitiesWithIncludes(DateTime startDate, DateTime endDate)
        {
            return _context.LeadActivities
                    .Where(a => a.ActivityDate >= startDate && a.ActivityDate <= endDate)
                    .Include(a => a.Lead)
                    .Include(a => a.Activity);
        }

        public void AddLeadActivity(LeadActivity leadActivity)
        {
            _context.LeadActivities.Add(leadActivity);
        }

        public LeadActivityRepository(IMarketoDbContext context)
        {
            _context = context;
        }

        public bool IsInDB(LeadActivity leadActivity)
        {
            return
                _context.LeadActivities.Any(la => la.ActivityDate == leadActivity.ActivityDate);
        }
    }
}
