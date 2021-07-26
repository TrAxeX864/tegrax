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
    public class TopAbsCommandHandler : CommandHandler
    {
        public TopAbsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text.StartsWith("/topv"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();

            var splitData = update.Message.Text.Split(" ");

            AbsItemType? absItemType = null;

            if (splitData.Length == 2)
            {
                absItemType = Enum.Parse<AbsItemType>(splitData[1]);
            }

            if (absItemType == null)
            {
                absItemType = AbsItemType.Онанизм;
            }
            
            int offset = 0;
            
            var topUsers = absItemDbRepository.GetUsersTop(absItemType.Value, offset, 15);
            
            string txt = $"Список лучших пользователей по категории <b>{absItemType}</b>:\n\n";
            int pos = 1;
            
            foreach (var absItem in topUsers)
            {
                DateTime dateFrom = absItem.Started;
                TimeSpan span = DateTime.Now - dateFrom;

                string star = "";

                if (absItem.User.HasPremium)
                {
                    star = " ⭐️";
                }
            
                txt += pos++ + $".{star} <b>@{absItem.User.Username}</b> - <b>{(int)span.TotalDays}</b> {Speller.DaysString((int)span.TotalDays)}\n";
            }

            if (topUsers.Count == 0)
            {
                txt += "<i>Не найдено ни одного пользователя</i>";
            }
            
            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, txt, ParseMode.Html, replyToMessageId: update.Message.MessageId);
        }
    }
}