namespace PlayingWithMarketo.Core.Repositories
{
    public interface IExportJobRepository
    {
        string GetJobStatus(string jobId);
    }
}
