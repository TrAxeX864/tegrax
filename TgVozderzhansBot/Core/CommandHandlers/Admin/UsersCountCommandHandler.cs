using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class UsersCountCommandHandler : CommandHandler
    {
        public UsersCountCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text == "/users_count")
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            int usersCount = userDbRepository.GetUsersCount();

            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Текущее количество пользователей в системе: " + usersCount);
        }
    }
}