using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Message)]
    public class InputFriendCommandHandler : CommandHandler
    {
        public InputFriendCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();
            FriendDbRepository friendDbRepository = new FriendDbRepository();
            
            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (friendDbRepository.HasInputFriendNickname(user))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            string friendNickname = update.Message.Text;
            
            UserDbRepository userDbRepository = new UserDbRepository();
            FriendDbRepository friendDbRepository = new FriendDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);
            
            friendDbRepository.AbortInputFriendNickname(user);

            var friendItem = new FriendItem();
            friendItem.User = user;

            var friend = friendDbRepository.FindNotConfirmedFriend(friendNickname);

            if (friend == null)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Друг, которого вы собираетесь пригласить не найден в боте. Возможно вы ошиблись в вводе имени.");
                return;
            }

            if (friendDbRepository.HasConfirmedFriend(user, friendNickname))
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Этот друг уже есть в списке ваших друзей.");
                return;
            } 
            
            friendItem.Friend = friend;
            
            friendDbRepository.InviteFriend(friendItem);

            await TelegramBotClient.SendTextMessageAsync(friendItem.Friend.ChatId, 
                $"Вас пригласили в друзья. Никнейм пользователя: @{friendItem.User.Username}", replyMarkup: new InlineKeyboardMarkup(
                    new []
                    {
                        new InlineKeyboardButton
                        {
                            Text = "Одобрить приглашение",
                            CallbackData = $"friend-approve:{friendItem.Id}"
                        },
                        new InlineKeyboardButton
                        {
                            Text = "Отменить",
                            CallbackData = $"cancel-friend-approve:{friendItem.Id}"
                        }
                    }
                ));
            
            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Вы успешно отправили приглашение другу");
        }
    }
}