using System;

namespace TgVozderzhansBot.Models
{
    public class InputMessageToFriend
    {
        public long Id { get; set; }
        
        public User User { get; set; }
        
        public User Friend { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}