using System;
using System.ComponentModel.DataAnnotations;

namespace PlayingWithMarketo.Core.Models
{
    public class Log
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [MaxLength(255)]
        public string Method { get; set; }
        [Required]
        [MaxLength(50)]
        public string Level { get; set; }
        [Required]
        [MaxLength(255)]
        public string Logger { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Message { get; set; }
        [MaxLength(2000)]
        public string Exception { get; set; }
    }
}
