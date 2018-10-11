using PlayingWithMarketo.Core;
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

        public UnitOfWork(MarketoDbContext context)
        {
            _context = context;
            LeadActivities = new LeadActivityRepository(_context);
            Activities = new ActivityRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
