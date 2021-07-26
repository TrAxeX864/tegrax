using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TegraXTest.Core.CommandHandlers
{
    [CommandType(CmdType.Message)]
    public class StartCommandHandler : CommandHandler
    {
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
            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Привет");
        }

        public StartCommandHandler(TelegramBotClient bot) : base(bot)
        {
            
        }
    }
}