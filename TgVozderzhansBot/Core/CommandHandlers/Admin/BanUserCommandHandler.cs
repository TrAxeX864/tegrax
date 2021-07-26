using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class BanUserCommandHandler : CommandHandler
    {
        public BanUserCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text != null && update.Message.Text.StartsWith("/ban_user"))
                {
                    return true;
                }

                if (update.Message.Text != null && update.Message.Text.StartsWith("/unban_user"))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.Message.Text.Split(" ");

            string nickname = splitData[1];
            
            UserDbRepository userDbRepository = new UserDbRepository();

            bool result = false;
            
            if (splitData[0].Contains("/ban_user"))
            {
                result = userDbRepository.BanUser(nickname, true);
            } 
            else if (splitData[0].Contains("/unban_user"))
            {
                result = userDbRepository.BanUser(nickname, false);
            }

            if (result)
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Команда успешно выполнена.");
            else
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Не удалось выполнить команду. Вероятнее всего вы ввели неправильный никнейм пользователя, которого собираетесь забанить.");
        }
    }
}