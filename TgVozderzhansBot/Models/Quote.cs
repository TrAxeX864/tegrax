using System;
using System.ComponentModel.DataAnnotations;

namespace TgVozderzhansBot.Models
{
    public class Quote
    {
        public long Id { get; set; }
        
        [MaxLength(3000)]
        [Required]
        public string Text { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}