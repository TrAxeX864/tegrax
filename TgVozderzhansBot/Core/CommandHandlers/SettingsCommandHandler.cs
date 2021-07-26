using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Message)]
    public class SettingsCommandHandler : CommandHandler
    {
        private bool _updateFlag = false;
        
        public SettingsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public void EnableUpdateFlag()
        {
            _updateFlag = true;
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message != null && update.Message.Text.EndsWith("Настройки"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            long? chatId = null;

            if (update.Message != null)
            {
                chatId = update.Message.Chat.Id;
            }
            else
            {
                chatId = update.CallbackQuery.Message.Chat.Id;
            }
            
            var user = userDbRepository.GetUser(chatId.Value);
            
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

            if (user.PublicAccount)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    CallbackData = "set_private_account",
                    Text = "Публичный аккаунт"
                });   
            }
            else
            {
                buttons.Add(new InlineKeyboardButton
                {
                    CallbackData = "set_public_account",
                    Text = "Приватный аккаунт"
                });
            }

            if (!_updateFlag)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Настройки аккаунта:", ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons.Select(x => new InlineKeyboardButton[] {x})));
            }
            else
            {
                await TelegramBotClient.EditMessageTextAsync(new ChatId(update.CallbackQuery.Message.Chat.Id), update.CallbackQuery.Message.MessageId, "Настройки аккаунта:", replyMarkup: new InlineKeyboardMarkup(buttons.Select(x => new InlineKeyboardButton[] {x})));
            }
        }
    }
}