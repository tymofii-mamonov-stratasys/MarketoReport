using PlayingWithMarketo.Core.Repositories;
using System.Linq;

namespace PlayingWithMarketo.Persistance.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMarketoDbContext _context;

        public ActivityRepository(IMarketoDbContext context)
        {
            _context = context;
        }

        public string GetActivity(int activityId)
        {
            return
                _context.Activities.Single(a => a.Id == activityId).ActivityName;
        }

        public int GetId(int activityId)
        {
            return
                _context.Activities.Single(a => a.ActivityId == activityId).Id;
        }
    }
}
