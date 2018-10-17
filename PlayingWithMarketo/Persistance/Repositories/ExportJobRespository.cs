using PlayingWithMarketo.Core.Models;
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

        public void AddExportJob(ExportJob exportJob)
        {
            _context.ExportJobs.Add(exportJob);
        }

        public ExportJob GetExportJob(string jobId)
        {
            return
                _context.ExportJobs.SingleOrDefault(j => j.ExportId == jobId);
        }

        public string GetJobStatus(string jobId)
        {
            return
                _context.ExportJobs.SingleOrDefault(j => j.ExportId == jobId).Status;
        }
    }
}
