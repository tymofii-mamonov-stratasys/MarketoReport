﻿using PlayingWithMarketo.Core.Models;
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

        public LeadActivityRepository(IMarketoDbContext context)
        {
            _context = context;
        }
    }
}
