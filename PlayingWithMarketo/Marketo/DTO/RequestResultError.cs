using System.Collections.Generic;

namespace PlayingWithMarketo.Marketo.DTO
{
    public class RequestResultError
    {
        public string requestId { get; set; }
        public List<Error> errors { get; set; }
        public bool success { get; set; }

    }
}
