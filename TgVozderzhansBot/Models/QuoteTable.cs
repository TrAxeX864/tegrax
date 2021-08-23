using System;

namespace TgVozderzhansBot.Models
{
    public class QuoteTable
    {
        public long Id { get; set; }
        
        public User User { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public Quote LastQuote { get; set; }
    }
}