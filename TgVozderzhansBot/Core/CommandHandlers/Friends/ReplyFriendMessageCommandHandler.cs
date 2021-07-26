using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class ReplyFriendMessageCommandHandler : CommandHandler
    {
        public ReplyFriendMessageCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("reply_message"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.CallbackQuery.Data.Split(":");

            long friendItemId = long.Parse(splitData[1]);

            update.CallbackQuery.Data = $"r-friend-send-message:{friendItemId}";

            var commandHandler = new SendMessageToFriendCommandHandler(TelegramBotClient);

            await commandHandler.Handler(update);

            await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
        }
    }
}