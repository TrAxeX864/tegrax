using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class DeletePremiumCommandHandler : CommandHandler
    {
        public DeletePremiumCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text.StartsWith("/delete_premium "))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.Message.Text.Split(" ");
            
            UserDbRepository userDbRepository = new UserDbRepository();

            bool result = userDbRepository.DeletePremiumUser(splitData[1]);

            if (result)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Команда успешно выполнена.");
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Не удалось лишить пользователя премиума, пользователь на найден.");
            }
        }
    }
}