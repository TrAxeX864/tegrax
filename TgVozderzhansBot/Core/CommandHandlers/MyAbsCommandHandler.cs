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
    [CommandType(CmdType.Message)]
    public class MyAbsCommandHandler : CommandHandler
    {
        public MyAbsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text != null && update.Message.Text.EndsWith("Мое Воздержание"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>();

            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            // Если есть какое-либо воздержание
            bool hasAbstinence = absItemDbRepository.HasAbstinence(user);

            buttons.Add(new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    CallbackData = "start_abs",
                    Text = "Начать воздержание"
                }
            });

            if (hasAbstinence)
            {
                buttons.Add(
                    new List<InlineKeyboardButton>
                    {
                        new InlineKeyboardButton
                        {
                            CallbackData = "stop_abs",
                            Text = "Прервать воздержание"
                        }

                    });
            }
            
            buttons.Add(new List<InlineKeyboardButton>
            {
                 new InlineKeyboardButton
                 {
                     CallbackData = "abshistory",
                     Text = "История воздержания"
                 }
            });

            buttons.Add(new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    CallbackData = "my-friends",
                    Text = "Мои друзья"
                }
            });

            if (hasAbstinence)
            {
                buttons.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton
                    {
                        CallbackData = "myterm",
                        Text = "Мой срок"
                    }
                });
            }

            string text = "Выберите пункт меню:";

            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, text,
                replyMarkup: new InlineKeyboardMarkup(buttons));
        }
    }
}