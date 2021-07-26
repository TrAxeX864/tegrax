using System.Collections.Generic;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Message)]
    public class StartCommandHandler : CommandHandler
    {
        public StartCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text != null && update.Message.Text.StartsWith("/start"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.Message.Text.Split(" ");

            long? inviterChatId = null;
            
            // Парсим Chat Id пригласившего нас пользователя 
            if (splitData.Length == 2)
            {
                inviterChatId = long.Parse(splitData[1]);
            }
            
            // Если у пользователя нету никнеймма, то не позволяем пройти дальше
            if (update.Message.From.Username == null || update.Message.From.Username.Length <= 1)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Чтобы пользоваться ботом, у Вас должен быть установлен никнейм в телеграме. Например: @username");

                return;
            }
            
            string text = "<b>Воздержание</b> приветствует Вас.\n\nВ нем вы можете воздерживаться от вредных привычек, и соревноваться с другими пользователями.<b>Внимание!</b> Новым пользователям выдается премиум аккаунт в подарок на одну неделю.";

            IReplyMarkup markup = new ReplyKeyboardMarkup(
                keyboard: new []
                    {
                    
                        new KeyboardButton[]
                        {
                            new KeyboardButton {Text = "❤️ Мое Воздержание"},
                            new KeyboardButton {Text = "⚙️ Настройки"},
                        },
                        new KeyboardButton[]
                        {
                            new KeyboardButton {Text = "🎆 Топ Пользователей"},
                            new KeyboardButton {Text = "🎾 Премиум Аккаунт"}
                        } 
                    },
                    resizeKeyboard: true
                );

            UserDbRepository userDbRepository = new UserDbRepository();
            userDbRepository.CreateUserIfNotExists(update.Message.Chat.Id, update.Message.From.Username, update.Message.From.FirstName + " " + update.Message.From.LastName,
                inviterChatId: inviterChatId);

            var user = userDbRepository.GetUser(update.Message.Chat.Id);
            userDbRepository.GiftPremiumIfNewbie(user);
            
            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, text, ParseMode.Html, replyMarkup: markup);
        }
    }
}