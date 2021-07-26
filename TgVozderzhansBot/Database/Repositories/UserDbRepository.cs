using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Database.Repositories
{
    public class UserDbRepository
    {
        private readonly ApplicationContext _db = new ApplicationContext();
        
        public UserDbRepository()
        {
            
        }

        public enum AccountPrivacy
        {
            Public, Private
        }

        /**
         * Удалить премиум пользователю по никнейму
         */
        public bool DeletePremiumUser(string username)
        {
            var user = _db.Users.FirstOrDefault(x => x.Username == username);

            if (user != null)
            {
                user.PremiumActiveTo = null;

                _db.Users.Update(user);
                _db.SaveChanges();
                
                return true;
            }

            return false;
        }

        /**
         * Посмотреть сколько осталось дней премиум аккаунту
         */
        public TimeSpan? GetPremiumRemaining(User user)
        {
            if (user.PremiumActiveTo == null) return null;

            var timeSpan = user.PremiumActiveTo - DateTime.Now;

            if (timeSpan != null)
            {
                if (timeSpan.Value.TotalDays < 0)
                {
                    return null;
                }

                return timeSpan;
            }

            return null;
        }
        
        /**
         * Установить приватность аккаунта
         */
        public void SetAccountPrivacy(User user, bool isPublic)
        {
            user = _db.Users.FirstOrDefault(x => x.Id == user.Id);

            if (user != null)
            {
                user.PublicAccount = isPublic;

                _db.Users.Update(user);
                _db.SaveChanges();
            }
        }

        /**
         * Получить количество всех пользователей
         */
        public int GetUsersCount()
        {
            return _db.Users.Count();
        }

        /**
         * Получить всех пользователей
         */
        public List<User> GetAllUsers()
        {
            return _db.Users.OrderBy(x => x.Id).ToList();
        }
        
        /**
         * Получить пользователя из базы данных
         */
        public User GetUser(long chatId)
        {
            var user = _db.Users.Include(x => x.AbsItems).FirstOrDefault(x => x.ChatId == chatId);

            return user;
        }

        /**
         * Забанить пользователя по никнейму (или разбанить)
         * Если flag стоит true, то пользователь будет забанен, если
         * flag стоит false, то пользователь будбет разбанен
         */
        public bool BanUser(string nickname, bool flag)
        {
            var user = _db.Users.FirstOrDefault(x => x.Username == nickname && !x.IsAdmin);

            if (user != null)
            {
                user.IsBanned = flag;

                _db.Users.Update(user);
                _db.SaveChanges();

                return true;
            }

            return false;
        }
        
        /**
         * Получить юзера по никнейму
         */
        public User GetUserByNickname(string nickname)
        {
            var user = _db.Users.Include(x => x.AbsItems).FirstOrDefault(x => x.Username == nickname);

            return user;
        }

        /**
         * Дарит премиум аккаунт новичку на 7 дней, если аккаунт создан недавно
         */
        public void GiftPremiumIfNewbie(User user)
        {
            var timeSpan = DateTime.Now - user.CreatedAt;

            if (!user.HasPremium && timeSpan.TotalDays <= 5)
            {
                user.PremiumActiveTo = DateTime.Now.AddDays(7);

                _db.Users.Update(user);
                _db.SaveChanges();
            }
        }
        
        /**
         * Создает пользователя в базе данных, если такого еще нет
         * Пользователь создается по уникальному ключу chatId
         */
        public void CreateUserIfNotExists(long chatId, string username, string name, long? inviterChatId = null)
        {
            int c = _db.Users.Count(x => x.ChatId == chatId);

            if (c == 0)
            {
                var user = new User();
                user.Name = name;
                user.Username = username;
                user.ChatId = chatId;

                if (inviterChatId != null && _db.Users.Count(x => x.ChatId == chatId && x.InviterChatId == inviterChatId) == 0)
                {
                    user.InviterChatId = inviterChatId;
                }

                _db.Users.Add(user);

                _db.SaveChanges();
            }
            else
            {
                var user = _db.Users.FirstOrDefault(x => x.ChatId == chatId);

                if (user != null)
                {
                    user.Name = name;
                    user.Username = username;

                    _db.Users.Update(user);

                    _db.SaveChanges();
                }
            }
        }
    }
}