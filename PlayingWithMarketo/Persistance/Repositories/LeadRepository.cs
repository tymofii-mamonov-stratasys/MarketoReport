using PlayingWithMarketo.Core.Models;
using PlayingWithMarketo.Core.Repositories;
using System.Linq;

namespace PlayingWithMarketo.Persistance.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly IMarketoDbContext _context;

        public LeadRepository(IMarketoDbContext context)
        {
            _context = context;
        }

        public void AddLead(Lead lead)
        {
            _context.Leads.Add(lead);
        }

        public int GetId(int leadId)
        {
            return
                _context.Leads.Single(l => l.LeadId == leadId).Id;
        }

        public bool LeadIsInDb(int leadId)
        {
            return
                 _context.Leads.Any(l => l.LeadId == leadId);
        }
    }
}
