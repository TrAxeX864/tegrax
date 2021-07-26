using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class FriendsMenuCommandHandler : CommandHandler
    {
        public FriendsMenuCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data == "my-friends")
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var buttons = new List<InlineKeyboardButton>();
            
            buttons.Add(new InlineKeyboardButton
            {
                Text = "Добавить друга",
                CallbackData = "add-friend"
            });
            
            buttons.Add(new InlineKeyboardButton
            {
                Text = "Список друзей",
                CallbackData = "friend-list"
            });

            await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Выберите пункт меню:",
                ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(buttons.Select(x => new InlineKeyboardButton[] {x})));

            await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
        }
    }
}