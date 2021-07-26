using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Message)]
    public class InputMessageToFriendCommandHandler : CommandHandler
    {
        public InputMessageToFriendCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();
            FriendDbRepository friendDbRepository = new FriendDbRepository();
            
            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (friendDbRepository.HasInputFriendMessage(user))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            using ApplicationContext db = new ApplicationContext();
            
            UserDbRepository userDbRepository = new UserDbRepository();
            FriendDbRepository friendDbRepository = new FriendDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);
            
            var inputMessageToFriend = db.InputMessageToFriends
                .Include(x => x.User)
                .Include(x => x.Friend)
                .FirstOrDefault(x => x.User == user);

            friendDbRepository.AbortInputFriendMessage(user);

            if (inputMessageToFriend != null)
            {
                var friendMessageTo = inputMessageToFriend.Friend;
                string message = $"Вам пришло сообщение от друга @{user.Username}:\n\n" + update.Message.Text;

                var friendItem = friendDbRepository.FindFriendItemByFriendId(user, inputMessageToFriend.Friend.Id);
                
                var button = new InlineKeyboardButton
                {
                    Text = "Ответить на сообщение",
                    CallbackData = $"reply_message:{friendItem.Id}"
                };

                await TelegramBotClient.SendTextMessageAsync(friendMessageTo.ChatId, message, ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(button));

                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Сообщение успешно отправлено другу @{friendMessageTo.Username}.");
            }
        }
    }
}