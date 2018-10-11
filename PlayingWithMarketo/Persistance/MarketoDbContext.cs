using PlayingWithMarketo.Core.Models;
using System.Data.Entity;

namespace PlayingWithMarketo.Persistance
{
    public class MarketoDbContext : DbContext, IMarketoDbContext
    {
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ExportJob> ExportJobs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<LeadActivity> LeadActivities { get; set; }

        public MarketoDbContext() : base("MarketoData") { }
    }
}
