using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class AddFriendCommandHandler : CommandHandler
    {
        public AddFriendCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data == "add-friend")
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();
            FriendDbRepository friendDbRepository = new FriendDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            // if (!user.HasPremium && friendDbRepository.GetFriendsCount(user) >= 2)
            // {
            //     TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id,
            //         "Чтобы пригласить больше друзей вам нужен премиум аккаунт.", true);
            //
            //     return;
            // }
            
            string text = "Введите никнейм друга с собакой:";

            TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, ParseMode.Html);

            friendDbRepository.InputFriendNickname(user);

            TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
        }
    }
}