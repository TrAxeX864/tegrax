using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class CancelFriendApproveCommandHandler : CommandHandler
    {
        public CancelFriendApproveCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("cancel-friend-approve:"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.CallbackQuery.Data.Split(":");

            long friendInviteId = long.Parse(splitData[1]);
            
            FriendDbRepository friendDbRepository = new FriendDbRepository();

            friendDbRepository.DeleteFriendInvite(friendInviteId);
            
            await TelegramBotClient.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
        }
    }
}