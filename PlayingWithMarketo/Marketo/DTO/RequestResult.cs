using System.Collections.Generic;

namespace PlayingWithMarketo.Marketo.DTO
{
    public class RequestResult
    {
        public string requestId { get; set; }
        public List<Result> result { get; set; }
        public bool success { get; set; }

    }
}
