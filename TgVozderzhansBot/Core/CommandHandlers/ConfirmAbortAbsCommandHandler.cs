using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class ConfirmAbortAbsCommandHandler : CommandHandler
    {
        public ConfirmAbortAbsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("abort_abs_confirm"))
            {
                return true;
            }

            if (update.CallbackQuery.Data == "abort_abs_confirm_cancel")
            {
                return true;
            }

            if (update.CallbackQuery.Data.StartsWith("abort_abs_confirm_yes"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            string text = "Действительно ли вы хотите сорваться?";

            // Обработка отмены срыва
            if (update.CallbackQuery.Data == "abort_abs_confirm_cancel")
            {
                await TelegramBotClient.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id,
                    update.CallbackQuery.Message.MessageId);
                return;
            }
            
            var splitData = update.CallbackQuery.Data.Split(":");

            AbsItemType? absItemType = null;

            if (splitData.Length > 0)
            {
                absItemType = Enum.Parse<AbsItemType>(splitData[1]);
            }

            // Обработка срыва
            if (update.CallbackQuery.Data.StartsWith("abort_abs_confirm_yes"))
            {
                var commandHandler = new StopAbsCommandHandler(TelegramBotClient);
                update.CallbackQuery.Data = $"xstopabs_{absItemType}";
                await commandHandler.Handler(update);
                
                await TelegramBotClient.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id,
                    update.CallbackQuery.Message.MessageId);
                
                return;
            }

            var buttons = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    CallbackData = $"abort_abs_confirm_yes:{absItemType}",
                    Text = "Сорваться"
                },
                new InlineKeyboardButton
                {
                    CallbackData = "abort_abs_confirm_cancel",
                    Text = "Отменить"
                }
            };

            await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, text,
                ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
            
            try
            {
                await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
            }
            catch
            {
                
            }
        }
    }
}