using System;

namespace TgVozderzhansBot.Models
{
    public class InputFriendNickname
    {
        public long Id { get; set; }
        
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}