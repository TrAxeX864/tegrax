using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    /**
     * С помощью данной команды можно проверить сколько у вас осталось дней премиум аккаунта
     */
    [CommandType(CmdType.Message)]
    public class PremiumStatusCommandHandler : CommandHandler
    {
        public PremiumStatusCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text == "/premium")
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            var timeSpan = userDbRepository.GetPremiumRemaining(user);

            if (timeSpan == null)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "У вас нет премиума. Невозможно посчитать остаток его дней.");
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "До истечения премиума осталось дней: " + timeSpan.Value.TotalDays.ToString("F0"));
            }
        }
    }
}