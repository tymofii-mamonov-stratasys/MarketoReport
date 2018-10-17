using PlayingWithMarketo.Core.Models;

namespace PlayingWithMarketo.Core.Repositories
{
    public interface ILeadRepository
    {
        int GetId(int leadId);
        bool LeadIsInDb(int leadId);
        void AddLead(Lead lead);
    }
}
