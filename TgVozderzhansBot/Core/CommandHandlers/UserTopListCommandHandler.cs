using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class UserTopListCommandHandler : CommandHandler
    {
        public UserTopListCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("topabs:"))
            {
                return true;
            }
            
            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.StartsWith("users_top_nav"))
                {
                    return true;
                }
            }

            return false;
        }

        private const int ItemsOnPage = 10;

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
            
            var absItemType = Enum.Parse<AbsItemType>(update.CallbackQuery.Data.Split(":")[1]);

            //var items = _db.AbsItems.Where(x => x.Finished == null && x.AbsItemType == absItemType).ToList();
            
            int offset = 0;
            
            int totalUsersCount = absItemDbRepository.GetUsersTopCount(absItemType);
            
            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.Contains("nav"))
                {
                    offset = int.Parse(update.CallbackQuery.Data.Split(":").Last());
                    
                    if (offset < 0) offset = 0;
                }
            }

            var topUsers = absItemDbRepository.GetUsersTop(absItemType, offset, ItemsOnPage);
            
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

            if (offset + 1 > ItemsOnPage)
            {
                int newOffset = offset - ItemsOnPage;
                
                if (newOffset < 0) newOffset = 0;

                buttons.Add(new InlineKeyboardButton
                {
                    Text = "<<",
                    CallbackData = $"users_top_nav:{absItemType}:{newOffset}"
                });
            }
            
            if (ItemsOnPage+offset < totalUsersCount)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = ">>",
                    CallbackData = $"users_top_nav:{absItemType}:{(offset+ItemsOnPage)}"
                });
            }
            
            string txt = $"Список лучших пользователей по категории <b>{absItemType}</b>:\n\n";
            int pos = offset+1;
            
            foreach (var absItem in topUsers)
            {
                DateTime dateFrom = absItem.Started;
                TimeSpan span = DateTime.Now - dateFrom;

                string star = "";

                if (absItem.User.HasPremium)
                {
                    star = " ⭐️";
                }
            
                txt += pos++ + $".{star} <b>@{absItem.User.Username}</b> - <b>{(int)span.TotalDays}</b> {Speller.DaysString((int)span.TotalDays)}\n";
            }

            if (topUsers.Count == 0)
            {
                txt += "<i>Не найдено ни одного пользователя</i>";
            }
            
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.StartsWith("users_top_nav"))
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