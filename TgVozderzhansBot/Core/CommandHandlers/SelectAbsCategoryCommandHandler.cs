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
    public class SelectAbsCategoryCommandHandler : CommandHandler
    {
        public SelectAbsCategoryCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery != null && update.CallbackQuery.Data.StartsWith("start_abs"))
            {
                return true;
            }

            return false;
        }

        private const int ItemsOnPage = 6;

        public override async Task Handler(Update update)
        {
            string text = "Выберите категорию воздержания:";
            
            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
            
            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            int offset = 0;

            int totalCount = absItemDbRepository.GetAvailableAbstinenceCount(user);
            
            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.Contains("nav"))
                {
                    offset = int.Parse(update.CallbackQuery.Data.Split(":").Last());
                    
                    if (offset < 0) offset = 0;
                }
            }
            
            var items = absItemDbRepository.GetAvailableAbstinence(user, offset, ItemsOnPage);

            var navButtons = new List<InlineKeyboardButton>();

            if (offset + 1 > ItemsOnPage)
            {
                int newOffset = offset - ItemsOnPage;
                
                if (newOffset < 0) newOffset = 0;
                
                navButtons.Add(new InlineKeyboardButton
                {
                    Text = "<<",
                    CallbackData = $"start_abs_nav:{newOffset}"
                });
            }

            if (ItemsOnPage + offset < totalCount)
            {
                navButtons.Add(new InlineKeyboardButton
                {
                    Text = ">>",
                    CallbackData = $"start_abs_nav:{offset + ItemsOnPage}"
                });
            }

            var buttons = new List<InlineKeyboardButton>();

            foreach (var item in items)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = item.GetDescription(),
                    CallbackData = "startabs_" + item.ToString()
                });
            }

            var btnList = buttons.
                Select(x => new InlineKeyboardButton[] {x}).ToList();
            
            btnList.Add(navButtons.ToArray());
            
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.StartsWith("start_abs_nav"))
            {
                try
                { 
                    await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId,
                        text, ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(btnList));
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
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, 
                    replyMarkup: new InlineKeyboardMarkup(btnList));
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