using System;
using System.Linq;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Database.Repositories
{
    public class AbsGuardItemDbRepository
    {
        private ApplicationContext _db = new ApplicationContext();

        public void MakeNotify(User user, AbsItemType absItemType)
        {
            int count = _db.AbsGuardItems.Count(x => x.User == user && x.AbsItem.AbsItemType == absItemType);

            if (count == 0)
            {
                user = _db.Users.FirstOrDefault(x => x.Id == user.Id);
                
                var absItem = _db.AbsItems.FirstOrDefault(x => x.AbsItemType == absItemType && x.User == user && x.Finished == null);
            
                var absGuardItem = new AbsGuardItem();
                absGuardItem.User = user;
                absGuardItem.AbsItem = absItem;
                absGuardItem.LastNotify = DateTime.Now;

                _db.AbsGuardItems.Add(absGuardItem);
                _db.SaveChanges();   
            }
            else
            {
                var absGuardItem = _db.AbsGuardItems.FirstOrDefault(x => x.User == user && x.AbsItem.AbsItemType == absItemType);

                if (absGuardItem != null)
                {
                    absGuardItem.LastNotify = DateTime.Now;

                    _db.AbsGuardItems.Update(absGuardItem);
                    _db.SaveChanges();
                }
            }
        }
        
        public void ConfirmAbs(User user, AbsItemType absItemType)
        {
            int count = _db.AbsGuardItems.Count(x => x.User == user && x.AbsItem.AbsItemType == absItemType);

            if (count == 0)
            {
                user = _db.Users.FirstOrDefault(x => x.Id == user.Id);
                
                var absItem = _db.AbsItems.FirstOrDefault(x => x.AbsItemType == absItemType && x.User == user && x.Finished == null);
            
                var absGuardItem = new AbsGuardItem();
                absGuardItem.User = user;
                absGuardItem.AbsItem = absItem;
                absGuardItem.IsConfirm = true;
                absGuardItem.ConfirmDate = DateTime.Now;

                _db.AbsGuardItems.Add(absGuardItem);
                _db.SaveChanges();   
            }
            else
            {
                var absGuardItem = _db.AbsGuardItems.FirstOrDefault(x => x.User == user && x.AbsItem.AbsItemType == absItemType);

                if (absGuardItem != null)
                {
                    absGuardItem.IsConfirm = true;
                    absGuardItem.ConfirmDate = DateTime.Now;

                    _db.AbsGuardItems.Update(absGuardItem);
                    _db.SaveChanges();
                }
            }
        }
    }
}