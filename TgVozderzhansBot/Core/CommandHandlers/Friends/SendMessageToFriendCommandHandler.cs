using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class SendMessageToFriendCommandHandler : CommandHandler
    {
        public SendMessageToFriendCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("friend-send-message:"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.CallbackQuery.Data.Split(":");

            long friendItemId = long.Parse(splitData[1]);
            
            FriendDbRepository friendDbRepository = new FriendDbRepository();
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);
            var friendItem = friendDbRepository.FindFriendItem(friendItemId);
            
            string text = $"Введите сообщение, которое хотите отправить другу @{friendItem.Friend.Username}:";

            using ApplicationContext db = new ApplicationContext();
            
            InputMessageToFriend inputMessageToFriend = new InputMessageToFriend();
            inputMessageToFriend.User = db.Users.FirstOrDefault(x => x.Id == user.Id);
            inputMessageToFriend.Friend = db.Users.FirstOrDefault(x => x.Id == friendItem.Friend.Id);

            if (db.InputMessageToFriends.Count(x => x.User == user) == 0)
            {
                db.InputMessageToFriends.Add(inputMessageToFriend);
                db.SaveChanges();
            }
            
            await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, ParseMode.Html);

            await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
        }
    }
}