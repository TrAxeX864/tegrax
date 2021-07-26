using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeleticaOnlineX.Core.CommandHandlers
{
    public class Start : CommandHandler
    {
        public Start(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text == "/start")
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
        }
    }
}