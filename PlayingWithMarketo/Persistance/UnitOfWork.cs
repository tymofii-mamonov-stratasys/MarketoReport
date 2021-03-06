﻿using PlayingWithMarketo.Core;
using PlayingWithMarketo.Core.Repositories;
using PlayingWithMarketo.Persistance.Repositories;

namespace PlayingWithMarketo.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketoDbContext _context;
        public ILeadActivityRepository LeadActivities { get; set; }
        public IActivityRepository Activities { get; set; }
        public IExportJobRepository ExportJobs { get; set; }
        public ITokenRepository Tokens { get; set; }
        public ILeadRepository Leads { get; set; }

        public UnitOfWork(MarketoDbContext context)
        {
            _context = context;
            LeadActivities = new LeadActivityRepository(_context);
            Activities = new ActivityRepository(_context);
            ExportJobs = new ExportJobRespository(_context);
            Tokens = new TokenRepository(_context);
            Leads = new LeadRepository(_context);
        }

        public void Complete()
        {
            //_context.GetValidationErrors();
            _context.SaveChanges();
        }
    }
}
