using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using TgVozderzhansBot.Models;
using User = TgVozderzhansBot.Models.User;

namespace TgVozderzhansBot.Database.Repositories
{
    public class FriendDbRepository
    {
        private readonly ApplicationContext _db = new ApplicationContext();

        /**
         * Получить количество друзей
         */
        public int GetFriendsCount(User user)
        {
            return _db.FriendItems.Count(x => x.IsConfirm && x.User == user);
        }
        
        public bool DeleteFriend(User user, long friendItemId)
        {
            var friendItem = _db.FriendItems
                .Include(x => x.User)
                .Include(x => x.Friend)
                .FirstOrDefault(x => x.Id == friendItemId && x.IsConfirm && x.User == user);

            if (friendItem != null)
            {
                var mirrorFriendItem = _db.FriendItems.FirstOrDefault(x =>
                    x.IsConfirm && x.Friend == friendItem.User && x.User == friendItem.Friend);

                if (mirrorFriendItem != null)
                {
                    _db.FriendItems.Remove(friendItem);
                    _db.FriendItems.Remove(mirrorFriendItem);

                    _db.SaveChanges();

                    return true;
                }
            }

            return false;
        }
        
        public FriendItem FindFriendItem(long id)
        {
            return _db.FriendItems
                .Include(x => x.User)
                .Include(x => x.Friend)
                .FirstOrDefault(x => x.Id == id);
        }

        public FriendItem FindFriendItemByFriendId(User user, long friendId)
        {
            var item =  _db.FriendItems
                .Include(x => x.User)
                .Include(x => x.Friend)
                .FirstOrDefault(x => x.Friend.Id == user.Id && x.IsConfirm && x.User.Id == friendId);

            return item;
        }
        
        public List<FriendItem> GetMyFriendsList(User user, int offset, int limit)
        {
            var list = _db.FriendItems
                .Include(x => x.User)
                .Include(x => x.Friend)
                .Where(x => x.User == user && x.IsConfirm)
                .Skip(offset)
                .Take(limit)
                .ToList();

            return list;
        }

        public int GetMyFriendListCount(User user)
        {
            var count = _db.FriendItems.Count(x => x.User == user && x.IsConfirm);

            return count;
        }
        
        public void DeleteFriendInvite(long friendInviteId)
        {
            var friendItem = _db.FriendItems.FirstOrDefault(x => !x.IsConfirm && x.Id == friendInviteId);

            if (friendItem != null)
            {
                _db.FriendItems.Remove(friendItem);
                _db.SaveChanges();
            }
        }

        public bool ApproveFriendInvite(long friendItemId)
        {
            var friendItem = _db.FriendItems
                .Include(x => x.Friend)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == friendItemId);

            if (friendItem != null)
            {
                friendItem.IsConfirm = true;
                
                var mirrorFriendItem = new FriendItem();
                mirrorFriendItem.Friend = friendItem.User;
                mirrorFriendItem.User = friendItem.Friend;
                mirrorFriendItem.IsConfirm = true;

                _db.FriendItems.Add(mirrorFriendItem);
                _db.FriendItems.Update(friendItem);

                _db.SaveChanges();

                return true;
            }

            return false;
        }
        
        public void InviteFriend(FriendItem friendItem)
        {
            friendItem.User = _db.Users.FirstOrDefault(x => x.Id == friendItem.User.Id);
            friendItem.Friend = _db.Users.FirstOrDefault(x => x.Id == friendItem.Friend.Id);
            friendItem.IsConfirm = false;

            var oldRecords = _db.FriendItems.Where(x =>
                !x.IsConfirm && x.Friend == friendItem.Friend && x.User == friendItem.User);

            _db.FriendItems.RemoveRange(oldRecords);
            
            _db.FriendItems.Add(friendItem);
            _db.SaveChanges();
        }

        public bool HasConfirmedFriend(User user, string friendNickname)
        {
            if (friendNickname != null)
            {
                if (friendNickname.Length > 1 && friendNickname[0] == '@')
                {
                    friendNickname = friendNickname.Substring(1);
                }

                var count = _db.FriendItems.Count(x =>
                    x.IsConfirm && x.User == user && x.Friend.Username == friendNickname);

                return count > 0;
            }

            return false;
        }
        
        public User FindNotConfirmedFriend(string friendNickname)
        {
            if (friendNickname != null)
            {
                if (friendNickname.Length > 1 && friendNickname[0] == '@')
                {
                    friendNickname = friendNickname.Substring(1);
                }

                var friend = _db.Users.FirstOrDefault(x => x.Username == friendNickname);

                return friend;
            }

            return null;
        }
        
        public void AbortInputFriendNickname(User user)
        {
            var item = _db.InputFriendNicknames.FirstOrDefault(x => x.User == user);

            if (item != null)
            {
                _db.InputFriendNicknames.Remove(item);
                _db.SaveChanges();
            }
        }
        
        public void AbortInputFriendMessage(User user)
        {
            var item = _db.InputMessageToFriends.FirstOrDefault(x => x.User == user);

            if (item != null)
            {
                _db.InputMessageToFriends.Remove(item);
                _db.SaveChanges();
            }
        }
        
        public bool HasInputFriendNickname(User user)
        {
            return _db.InputFriendNicknames.Count(x => x.User == user) > 0;
        }

        public bool HasInputFriendMessage(User user)
        {
            return _db.InputMessageToFriends.Count(x => x.User == user) > 0;
        }
        
        public void InputFriendNickname(User user)
        {
            user = _db.Users.FirstOrDefault(x => x.Id == user.Id);

            if (_db.InputFriendNicknames.Count(x => x.User == user) == 0)
            {
                _db.InputFriendNicknames.Add(new InputFriendNickname
                {
                    User = user
                });

                _db.SaveChanges();    
            }
            else
            {
                var inputFriendNickname = _db.InputFriendNicknames.FirstOrDefault(x => x.User == user);

                if (inputFriendNickname != null)
                {
                    _db.InputFriendNicknames.Update(inputFriendNickname);
                    _db.SaveChanges();
                }
            }
        }
    }
}