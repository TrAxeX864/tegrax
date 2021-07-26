using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Database.Repositories
{
    public class AbsItemDbRepository
    {
        private readonly ApplicationContext _db = new ApplicationContext();

        /**
         * Проверяет воздерживается ли пользователь в данный момент
         * Если нет, то вернет false
         * В параметрах нужно указать тип воздержания
         */
        public bool HasAbstinence(User user)
        {
            int c = _db.AbsItems.Count(x => x.User == user && x.Finished == null);

            return c > 0;
        }

        /**
         * Получить текущий срок воздержания
         */
        public TimeSpan? GetCurrentTerm(User user, AbsItemType absItemType)
        {
            var item = _db.AbsItems.FirstOrDefault(x => x.User == user && x.AbsItemType == absItemType && x.Finished == null);

            if (item != null)
            {
                
                TimeSpan span = DateTime.Now - item.Started;

                return span;
            }

            return null;
        }
        
        /**
         * Стартануть новое воздержание
         */
        public bool StartNewAbstinence(User user, AbsItemType absItemType)
        {
            int absCount = _db.AbsItems.Count(x => x.Finished == null && x.User == user && x.AbsItemType == absItemType);

            if (absCount == 0)
            {
                if (user == null) return false;
                
                user = _db.Users.FirstOrDefault(x => x.Id == user.Id);
                
                AbsItem absItem = new AbsItem();
                absItem.Finished = null;
                absItem.Started = DateTime.Now;
                absItem.User = user;
                absItem.AbsItemType = absItemType;

                _db.AbsItems.Add(absItem);
                _db.SaveChanges();

                return true;
            }

            return false;
        }

        /**
         * Прервать воздержание определенного типа
         */
        public bool AbortAbstinence(User user, AbsItemType itemType)
        {
            var item =
                _db.AbsItems.FirstOrDefault(x => x.User == user && x.Finished == null && x.AbsItemType == itemType);

            var absGuardItem = _db.AbsGuardItems.FirstOrDefault(x => x.User == user && x.AbsItem.AbsItemType == itemType);
            
            if (item != null)
            {
                item.Finished = DateTime.Now;

                if (absGuardItem != null)
                {
                    _db.AbsGuardItems.Remove(absGuardItem);
                }
                
                _db.AbsItems.Update(item);
                _db.SaveChanges();
                
                return true;
            }

            return false;
        }
        
        /**
         * Получить текущие пункты воздержания
         */
        public List<AbsItemType> GetCurrentAbstinenceList(User user)
        {
            var currentAbsItems = _db.AbsItems.Where(x => x.User == user && x.Finished == null).Select(x => x.AbsItemType).ToList();

            return currentAbsItems;
        }

        /**
         * Получить все типы воздержания
         */
        public List<AbsItemType> GetAllAbsItemTypes(int offset = 0, int limit = 100)
        {
            var list = Enum.GetValues<AbsItemType>().Skip(offset).Take(limit).ToList();
            
            return list;
        }

        public int GetAllAbsItemTypesCount()
        {
            return Enum.GetValues<AbsItemType>().Length;
        }

        /**
         * Получить количество юзеров находящихся на воздержании
         */
        public int GetUsersCount(AbsItemType absItemType)
        {
            return _db.AbsItems.Count(x => x.Finished == null && x.AbsItemType == absItemType && x.User.PublicAccount);
        }

        /**
         * Получить историю воздержания
         */
        public List<AbsItem> GetAbsHistoryItems(User user, AbsItemType absItemType, int offset, int limit, bool orderByDesc = false)
        {
            var query = _db.AbsItems.Include(x => x.User).Where(x => x.User == user && x.Finished != null && x.AbsItemType == absItemType)
                .OrderBy(x => x.Id);

            if (orderByDesc)
            {
                query = query.OrderByDescending(x => x.Id);
            }

            query = (IOrderedQueryable<AbsItem>) query.Skip(offset).Take(limit);
            
            var list = query.ToList();

            return list;
        }

        /**
         * Получить количество элементов из истории воздержания 
         */
        public int GetAbsHistoryItemsCount(User user, AbsItemType absItemType)
        {
            int count = _db.AbsItems.Count(x => x.Finished != null && x.User == user && x.AbsItemType == absItemType);

            return count;
        }
        
        /**
         * Получить топ юзеров
         */
        public List<AbsItem> GetUsersTop(AbsItemType absItemType, int offset, int limit)
        {
            var list = _db.AbsItems.Include(x => x.User).
                Where(x =>
                    !x.User.IsBanned &&
                    x.AbsItemType == absItemType && x.Finished == null && x.User.PublicAccount).OrderBy(x => x.Started) .Skip(offset).Take(limit).ToList();

            return list;
        }

        public int GetUsersTopCount(AbsItemType absItemType)
        {
            var count = _db.AbsItems.
                Count(x =>
                    !x.User.IsBanned &&
                    x.AbsItemType == absItemType && x.Finished == null && x.User.PublicAccount);

            return count;
        }
        
        public int GetAvailableAbstinenceCount(User user)
        {
            var currentAbsItems = _db.AbsItems.Where(x => x.User == user && x.Finished == null).Select(x => x.AbsItemType).ToList();
            var totalAbsItems = Enum.GetValues<AbsItemType>().ToList();

            var newList = new List<AbsItemType>(totalAbsItems);
            
            foreach (var cItem in currentAbsItems)
            {
                foreach (var item in totalAbsItems)
                {
                    if (cItem == item)
                    {
                        newList.Remove(cItem);
                    }
                }
            }

            int count = newList.Count;

            return count;
        }
        
        /**
         * Получить доступные пункты воздержания
         */
        public List<AbsItemType> GetAvailableAbstinence(User user, int offset = 0, int limit = 100)
        {
            var currentAbsItems = _db.AbsItems.Where(x => x.User == user && x.Finished == null).Select(x => x.AbsItemType).ToList();
            var totalAbsItems = Enum.GetValues<AbsItemType>().ToList();

            var newList = new List<AbsItemType>(totalAbsItems);
            
            foreach (var cItem in currentAbsItems)
            {
                foreach (var item in totalAbsItems)
                {
                    if (cItem == item)
                    {
                        newList.Remove(cItem);
                    }
                }
            }

            newList = newList.OrderBy(x => (int) x).Skip(offset).Take(limit).ToList();

            return newList;
        }
    }
}