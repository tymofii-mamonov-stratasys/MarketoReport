using PlayingWithMarketo.Core.Repositories;
using System.Linq;

namespace PlayingWithMarketo.Persistance.Repositories
{
    public class ExportJobRespository : IExportJobRepository
    {
        private readonly IMarketoDbContext _context;
        public ExportJobRespository(IMarketoDbContext context)
        {
            _context = context;
        }

        public string GetJobStatus(string jobId)
        {
            return
                _context.ExportJobs.SingleOrDefault(j => j.ExportId == jobId).Status;
        }
    }
}
