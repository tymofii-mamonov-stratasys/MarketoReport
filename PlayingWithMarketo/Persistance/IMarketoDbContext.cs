using PlayingWithMarketo.Core.Models;
using System.Data.Entity;

namespace PlayingWithMarketo.Persistance
{
    public interface IMarketoDbContext
    {
        DbSet<Token> Tokens { get; set; }
        DbSet<ExportJob> ExportJobs { get; set; }
        DbSet<Log> Logs { get; set; }
        DbSet<Lead> Leads { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<LeadActivity> LeadActivities { get; set; }
    }
}
