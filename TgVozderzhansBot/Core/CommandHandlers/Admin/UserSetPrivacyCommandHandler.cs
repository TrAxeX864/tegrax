using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class UserSetPrivacyCommandHandler : CommandHandler
    {
        public UserSetPrivacyCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text.StartsWith("/set_privacy "))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.Message.Text.Split(" ");

            string username = splitData[1];
            bool flag = bool.Parse(splitData[2]);
            
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUserByNickname(username);

            if (user == null)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Пользователю, которому вы собираетесь выставить приватность аккаунта, не найден.");
                return;
            }
            
            userDbRepository.SetAccountPrivacy(user, flag);

            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Команда успешно выполнена.");
        }
    }
}