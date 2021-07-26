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
    [CommandType(CmdType.All)]
    public class UserTopCommandHandler : CommandHandler
    {
        public UserTopCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Text != null && update.Message.Text.EndsWith("Топ Пользователей"))
                {
                    return true;
                } 
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.StartsWith("userstop"))
                {
                    return true;
                }
            }
            
            return false;
        }

        private const int ItemsOnPage = 6;

        public override async Task Handler(Update update)
        {
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
            
            string caption = "Выберите ТОП воздержания:";

            int totalCount = absItemDbRepository.GetAllAbsItemTypesCount();
            
            var absItemTypes = absItemDbRepository.GetAllAbsItemTypes(offset, ItemsOnPage);
            
            var navButtons = new List<InlineKeyboardButton>();
            
            if (offset + 1 > ItemsOnPage)
            {
                int newOffset = offset - ItemsOnPage;
                
                if (newOffset < 0) newOffset = 0;
                
                navButtons.Add(new InlineKeyboardButton
                {
                    Text = "<<",
                    CallbackData = $"userstop_nav:{newOffset}"
                });
            }

            if (ItemsOnPage + offset < totalCount)
            {
                navButtons.Add(new InlineKeyboardButton
                {
                    Text = ">>",
                    CallbackData = $"userstop_nav:{offset + ItemsOnPage}"
                });
            }
            
            var buttons = new List<InlineKeyboardButton>();

            foreach (var item in absItemTypes)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = item.GetDescription(),
                    CallbackData = "topabs:" + item.ToString()
                });
            }
            
            var btnList = buttons.
                Select(x => new InlineKeyboardButton[] {x}).ToList();
            
            btnList.Add(navButtons.ToArray());

            long? chatId = null;

            if (update.Type == UpdateType.CallbackQuery)
            {
                chatId = update.CallbackQuery.Message.Chat.Id;
            } else if (update.Type == UpdateType.Message)
            {
                chatId = update.Message.Chat.Id;
            }
            
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.StartsWith("userstop_nav"))
            {
                try
                { 
                    await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId,
                        caption, ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(btnList));
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
                await TelegramBotClient.SendTextMessageAsync(chatId, caption,
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