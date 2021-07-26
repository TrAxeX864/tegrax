using System;
using System.Threading;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class StopAbsCommandHandler : CommandHandler
    {
        public StopAbsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }
        
        private async Task NotifyAllFriendsToMyBreakdown(Models.User user, AbsItemType absItemType)
        {
            using ApplicationContext db = new ApplicationContext();
            
            FriendDbRepository friendDbRepository = new FriendDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
            
            var friendList = friendDbRepository.GetMyFriendsList(user, 0, 100);

            foreach (var friend in friendList)
            {
                try
                {
                    long friendChatId = friend.Friend.ChatId;

                    var currentTerm = absItemDbRepository.GetCurrentTerm(user, absItemType);

                    await TelegramBotClient.SendTextMessageAsync(friendChatId, 
                        $"Ваш друг @{user.Username} сорвался <i>({absItemType})</i>. Он продержался <b>{currentTerm.Value.TotalDays:F0} {Speller.DaysString((int) currentTerm.Value.TotalDays)}.</b>", ParseMode.Html);
                }
                catch
                {
                    
                }
            }
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data != null && 
                (update.CallbackQuery.Data.StartsWith("stopabs_")))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var absItemType = Enum.Parse<AbsItemType>(update.CallbackQuery.Data.Split("_")[1]);
            
            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            await NotifyAllFriendsToMyBreakdown(user, absItemType);

            bool result = absItemDbRepository.AbortAbstinence(user, absItemType);

            if (result)
            {
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"Воздержание \"{absItemType}\" прервано");
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"Не удалось прервать воздержание");
            }
            
            try
            {
                await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
            }
            catch
            {
                
            }
        }
    }
}