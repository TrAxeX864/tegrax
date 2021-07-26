using System;

namespace TgVozderzhansBot.Models
{
    public class MailingItem
    {
        public long Id { get; set; }
        
        public User Owner { get; set; }
        
        public string Message { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public int TotalUsers { get; set; }
        
        public int Position { get; set; }
        
        public DateTime? FinishedAt { get; set; }
    }
}