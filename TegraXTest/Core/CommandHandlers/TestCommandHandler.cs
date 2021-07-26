using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TegraXTest.Core.CommandHandlers
{
    [CommandType(CmdType.Message)]
    public class TestCommandHandler : CommandHandler
    {
        public TestCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text.StartsWith("calc "))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            string[] splitData = update.Message.Text.Split(new char[] {' '});

            int x = int.Parse(splitData[1]);
            int y = int.Parse(splitData[2]);

            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Получилось: {x+y}");
        }
    }
}