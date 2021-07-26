using System;

namespace TgVozderzhansBot.Models
{
    public class AbsGuardItem
    {
        public long Id { get; set; }
        
        public User User { get; set; }
        
        public AbsItem AbsItem { get; set; }

        public DateTime? LastNotify { get; set; } = null;

        public DateTime? ConfirmDate { get; set; } = null;

        public bool IsConfirm { get; set; } = false;

        public int ConfirmMs { get; set; } = 60000;

        public bool Flag { get; set; } = false;

        public bool Flag2 { get; set; } = false;
    }
}