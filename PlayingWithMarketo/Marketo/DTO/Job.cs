using System.Collections.Generic;

namespace PlayingWithMarketo.Marketo.DTO
{
    public class Job
    {
        public string format { get; set; }
        public JobFilter filter { get; set; }
        public List<string> fields { get; set; }
    }
}
