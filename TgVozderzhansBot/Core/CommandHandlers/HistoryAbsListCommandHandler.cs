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
    public class HistoryAbsListCommandHandler : CommandHandler
    {
        public HistoryAbsListCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("history-abs:"))
            {
                return true;
            }

            if (update.CallbackQuery.Data.StartsWith("history_abs_nav"))
            {
                return true;
            }

            return false;
        }

        private const int ItemsOnPage = 10;
        
        public override async Task Handler(Update update)
        {
            var absItemType = Enum.Parse<AbsItemType>(update.CallbackQuery.Data.Split(":")[1]);
            
            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
            
            int offset = 0;
            
            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.Contains("nav"))
                {
                    offset = int.Parse(update.CallbackQuery.Data.Split(":").Last());
                    
                    if (offset < 0) offset = 0;
                }
            }

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            var historyItems = absItemDbRepository.GetAbsHistoryItems(user, absItemType, offset, ItemsOnPage);

            int historyItemsCount = absItemDbRepository.GetAbsHistoryItemsCount(user, absItemType);
            
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

            if (offset + 1 > ItemsOnPage)
            {
                int newOffset = offset - ItemsOnPage;
                
                if (newOffset < 0) newOffset = 0;

                buttons.Add(new InlineKeyboardButton
                {
                    Text = "<<",
                    CallbackData = $"history_abs_nav:{absItemType}:{newOffset}"
                });
            }
            
            if (ItemsOnPage + offset < historyItemsCount)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = ">>",
                    CallbackData = $"history_abs_nav:{absItemType}:{(offset+ItemsOnPage)}"
                });
            }
            
            string txt = $"История воздержания в категории <b>{absItemType}</b>:\n\n";
            int pos = offset+1;
            
            foreach (var historyItem in historyItems)
            {
                DateTime dateFrom = historyItem.Started;
                DateTime dateTo = historyItem.Finished.Value;

                TimeSpan timeSpan = dateTo - dateFrom;
                
                txt += $"<b>{pos++}</b>" + $". {dateFrom.ToString("yyyy-MM-dd HH:mm:ss")} - {dateTo.ToString("yyyy-MM-dd HH:mm:ss")} <i>({timeSpan.TotalDays:F0} дней)</i>\n";
            }

            if (historyItemsCount == 0)
            {
                txt += "<i>Не найдено истории воздержания</i>";
            }
            
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.StartsWith("history_abs_nav"))
            {
                try
                { 
                    await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId,
                        txt, ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                }
                catch
                {

                }
                finally
                {
                    await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                }
                
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, txt, ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
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