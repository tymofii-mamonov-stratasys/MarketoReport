using System.Collections.Generic;

namespace PlayingWithMarketo.Marketo.DTO
{
    public class JobFilter
    {
        public CreatedAt createdAt { get; set; }
        public List<int> activityTypeIds { get; set; }
    }
}
