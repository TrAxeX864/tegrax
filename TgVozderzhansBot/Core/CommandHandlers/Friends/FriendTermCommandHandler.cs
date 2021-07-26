using System;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class FriendTermCommandHandler : CommandHandler
    {
        public FriendTermCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("friend-term:"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.CallbackQuery.Data.Split(":");

            long friendItemId = long.Parse(splitData[1]);
            
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
            FriendDbRepository friendDbRepository = new FriendDbRepository();

            var friendItem = friendDbRepository.FindFriendItem(friendItemId);
            
            var currentAbstinenceList = absItemDbRepository.GetCurrentAbstinenceList(friendItem.Friend);

            string str = "";
            
            foreach (var currentAbs in currentAbstinenceList)
            {
                var span = absItemDbRepository.GetCurrentTerm(friendItem.Friend, currentAbs);

                if (span != null)
                {
                    var formatStr = String.Format ("<b>{0:00}</b> " + Speller.DaysString((int) span?.Days) + ", <b>{1:00}</b> " + Speller.HoursString((int) span?.Hours) + ", <b>{2:00}</b> " + Speller.MinutesString((int) span?.Minutes) + ", <b>{3:00}</b> " + Speller.SecondsString((int) span?.Seconds), span?.Days, span?.Hours, span?.Minutes, span?.Seconds);

                    str += $"Текущий срок воздержания <b>\"{currentAbs}\":</b>\n\n" + formatStr + "\n\n";
                } 
            }

            if (str == "")
            {
                str = $"<i>Ваш друг @{friendItem.Friend.Username} в данный момент не воздерживается.</i>";
            }
            
            await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                str, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton
                {
                    Text = "<< Вернуться назад",
                    CallbackData = $"navfriend:{friendItemId}"
                }));
            
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