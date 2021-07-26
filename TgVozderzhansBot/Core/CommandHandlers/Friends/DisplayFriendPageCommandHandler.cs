using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class DisplayFriendPageCommandHandler : CommandHandler
    {
        public DisplayFriendPageCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("navfriend:"))
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

            var friendItem = friendDbRepository.FindFriendItem(friendItemId);
            
            var buttons = new List<InlineKeyboardButton>();
            
            buttons.Add(new InlineKeyboardButton
            {
                Text = "Срок воздержания",
                CallbackData = $"friend-term:{friendItem.Id}"
            });
            
            buttons.Add(new InlineKeyboardButton
            {
                Text = "Отправить сообщение",
                CallbackData = $"friend-send-message:{friendItem.Id}"
            });
            
            buttons.Add(new InlineKeyboardButton
            {
                Text = "Удалить из друзей",
                CallbackData = $"friend-delete:{friendItem.Id}"
            });
            
            buttons.Add(new InlineKeyboardButton
            {
                Text = "<< Вернуться назад",
                CallbackData = "friend-list-back"
            });

            await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                $"Выберите пункт меню (@{friendItem.Friend.Username}):", replyMarkup: new InlineKeyboardMarkup(buttons.Select(x => new InlineKeyboardButton[] {x})));
        }
    }
}