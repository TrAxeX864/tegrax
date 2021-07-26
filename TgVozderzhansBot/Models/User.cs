using System;
using System.Collections.Generic;

namespace TgVozderzhansBot.Models
{
    public class User
    {
        public long Id { get; set; }
        
        public long ChatId { get; set; }
        
        public List<AbsItem> AbsItems { get; set; }
        
        public string Username { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsAdmin { get; set; } = false;

        public bool IsBanned { get; set; } = false;
        
        public DateTime? PremiumActiveTo { get; set; }

        public bool PublicAccount { get; set; } = true;

        public long? InviterChatId { get; set; } = null;

        public bool IsFriendGotPremium { get; set; } = false;
        
        public bool HasPremium
        {
            get
            {
                return PremiumActiveTo != null && DateTime.Now <= PremiumActiveTo;
            }
        }
    }
}