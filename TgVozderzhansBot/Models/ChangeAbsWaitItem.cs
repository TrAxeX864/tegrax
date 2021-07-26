namespace TgVozderzhansBot.Models
{
    public class ChangeAbsWaitItem
    {
        public long Id { get; set; }
        
        public User User { get; set; }
        
        public AbsItemType AbsItemType { get; set; }
    }
}