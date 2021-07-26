
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class MyTermListCommandHandler : CommandHandler
    {
        public MyTermListCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data != null && update.CallbackQuery.Data == "myterm")
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

            var currentAbsList = absItemDbRepository.GetCurrentAbstinenceList(user);

            string caption = "Выберите категорию воздержания:";
            
            var buttons = new List<InlineKeyboardButton>();

            foreach (var item in currentAbsList)
            {
                buttons.Add(new InlineKeyboardButton
                {
                    Text = item.ToString(),
                    CallbackData = "myterm-item_" + item.ToString()
                });
            }
            
            await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, caption, replyMarkup: new InlineKeyboardMarkup(buttons.Select(x => new InlineKeyboardButton[] {x}).ToList()));

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