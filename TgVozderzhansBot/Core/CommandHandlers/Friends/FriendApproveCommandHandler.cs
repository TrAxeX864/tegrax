using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class FriendApproveCommandHandler : CommandHandler
    {
        public FriendApproveCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("friend-approve:"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.CallbackQuery.Data.Split(":");

            long friendItemId = long.Parse(splitData[1]);
            
            FriendDbRepository friendDbRepository = new FriendDbRepository();

            var friendItem = friendDbRepository.FindFriendItem(friendItemId);
            
            bool result = friendDbRepository.ApproveFriendInvite(friendItemId);

            if (result)
            {
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Вы успешно добавили друга. Теперь вы можете обмениваться сообщениями. Смотреть срывы и срок воздержания друга.");

                await TelegramBotClient.SendTextMessageAsync(friendItem.User.ChatId, $"Ваш друг @{friendItem.Friend.Username} одобрил вашу заявку в друзья.");
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id,
                    "Не удалось добавить друга.");
            }

            await TelegramBotClient.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
        }
    }
}