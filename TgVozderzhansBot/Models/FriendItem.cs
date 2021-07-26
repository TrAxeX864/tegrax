using System;

namespace TgVozderzhansBot.Models
{
    public class FriendItem
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public User User { get; set; }
        
        public User Friend { get; set; }

        public bool IsConfirm { get; set; } = false;
    }
}