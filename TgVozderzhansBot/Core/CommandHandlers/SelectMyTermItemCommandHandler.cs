using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class SelectMyTermItemCommandHandler : CommandHandler
    {
        public SelectMyTermItemCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data != null && update.CallbackQuery.Data.StartsWith("myterm-item_"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);
            
            var absItemType = Enum.Parse<AbsItemType>(update.CallbackQuery.Data.Split("_")[1]);

            var span = absItemDbRepository.GetCurrentTerm(user, absItemType);

            if (span != null)
            {
                var str = String.Format ("<b>{0:00}</b> " + Speller.DaysString((int) span?.Days) + ", <b>{1:00}</b> " + Speller.HoursString((int) span?.Hours) + ", <b>{2:00}</b> " + Speller.MinutesString((int) span?.Minutes) + ", <b>{3:00}</b> " + Speller.SecondsString((int) span?.Seconds), span?.Days, span?.Hours, span?.Minutes, span?.Seconds);

                var button1 = new InlineKeyboardButton
                {
                    CallbackData = $"change_abs_term:{absItemType}",
                    Text = "Поменять срок воздержания"
                };

                var button2 = new InlineKeyboardButton
                {
                    CallbackData = "abort_abs_confirm:" + absItemType,
                    Text = "Я хочу сорваться"
                };

                var buttonList = new List<InlineKeyboardButton>();
                
                buttonList.Add(button1);
                buttonList.Add(button2);
                
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id,
                    $"Текущий срок воздержания \"{absItemType}\":\n\n" + str, ParseMode.Html, 
                    replyMarkup: new InlineKeyboardMarkup(buttonList.Select(x => new []{x})));
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id,
                    "Произошла ошибка", ParseMode.Html);
            }
            
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