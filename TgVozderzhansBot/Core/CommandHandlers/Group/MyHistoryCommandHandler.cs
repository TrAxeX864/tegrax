using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers.Group
{
    [CommandType(CmdType.Message)]
    [HandleOnlyFromGroup]
    public class MyHistoryCommandHandler : CommandHandler
    {
        public MyHistoryCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text.StartsWith("/myhistory"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.Message.Text.Split(" ");
            
            var absItemType = AbsItemType.Онанизм;

            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
            
            var user = userDbRepository.GetUser(update.Message.From.Id);

            // if (!user.HasPremium)
            // {
            //     await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
            //         "Чтобы посмотреть историю воздержания вам нужен премиум аккаунт.",
            //         replyToMessageId: update.Message.MessageId);
            //     return;
            // }
            
            var currentAbstinenceList = absItemDbRepository.GetCurrentAbstinenceList(user);

            if (currentAbstinenceList.Count > 0)
            {
                absItemType = currentAbstinenceList.FirstOrDefault();
            }
            
            if (splitData.Length == 2)
            {
                absItemType = Enum.Parse<AbsItemType>(splitData[1]);
            }
            
            var historyItems = absItemDbRepository.GetAbsHistoryItems(user, absItemType, 0, 10, orderByDesc: true);

            string txt = $"История воздержания в категории <b>{absItemType}</b>:\n\n";
            int pos = 1;
            
            foreach (var historyItem in historyItems)
            {
                DateTime dateFrom = historyItem.Started;
                DateTime dateTo = historyItem.Finished.Value;

                TimeSpan timeSpan = dateTo - dateFrom;
                
                txt += $"<b>{pos++}</b>" + $". {dateFrom:yyyy-MM-dd HH:mm:ss} - {dateTo:yyyy-MM-dd HH:mm:ss} <i>({timeSpan.TotalDays:F0} {Speller.DaysString((int)timeSpan.TotalDays)})</i>\n";
            }

            if (historyItems.Count == 0)
            {
                txt += "<i>Не найдено истории воздержания</i>";
            }
            
            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, txt, ParseMode.Html, replyToMessageId: update.Message.MessageId);
        }
    }
}