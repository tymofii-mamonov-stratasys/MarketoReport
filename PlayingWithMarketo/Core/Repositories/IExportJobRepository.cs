using PlayingWithMarketo.Core.Models;

namespace PlayingWithMarketo.Core.Repositories
{
    public interface IExportJobRepository
    {
        string GetJobStatus(string jobId);
        void AddExportJob(ExportJob exportJob);
        ExportJob GetExportJob(string jobId);
    }
}
