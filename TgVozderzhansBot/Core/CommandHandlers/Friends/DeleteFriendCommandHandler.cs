using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class DeleteFriendCommandHandler : CommandHandler
    {
        public DeleteFriendCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("friend-delete:"))
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

            var button = new InlineKeyboardButton
            {
                Text = $"<< Вернуться назад",
                CallbackData = "friend-list-back:0"
            };
            
            bool result = friendDbRepository.DeleteFriend(user, friendItemId);

            if (result)
            {
                await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                    "Пользователь успешно удален из друзей.", replyMarkup: new InlineKeyboardMarkup(button));
            }
            else
            {
                await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                    "Возникла ошибка! Не удалось удалить пользователя из друзей.", replyMarkup: new InlineKeyboardMarkup(button));
            }
        }
    }
}