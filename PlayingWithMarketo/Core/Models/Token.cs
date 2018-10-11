using System;
using System.ComponentModel.DataAnnotations;

namespace PlayingWithMarketo.Core.Models
{
    public class Token
    {
        public int Id { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
