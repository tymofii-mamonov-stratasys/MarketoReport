using PlayingWithMarketo.Core.Repositories;

namespace PlayingWithMarketo.Core
{
    public interface IUnitOfWork
    {
        ILeadActivityRepository LeadActivities { get; set; }
        IActivityRepository Activities { get; set; }
        IExportJobRepository ExportJobs { get; set; }
        ITokenRepository Tokens { get; set; }
        ILeadRepository Leads { get; set; }
        void Complete();
    }
}
