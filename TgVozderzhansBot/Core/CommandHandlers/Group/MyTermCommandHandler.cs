using System;
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
    public class MyTermCommandHandler : CommandHandler
    {
        public MyTermCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text != null && update.Message.Text.StartsWith("/myterm"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            long chatId = update.Message.From.Id;
            
            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();

            var user = userDbRepository.GetUser(chatId);

            var splitData = update.Message.Text.Split(" ");

            AbsItemType? absItemType = null;

            if (splitData.Length == 2)
            {
                absItemType = Enum.Parse<AbsItemType>(splitData[1]);
            }

            if (absItemType == null)
            {
                var absItem = user.AbsItems.OrderBy(x => x.Id).FirstOrDefault(x => x.Finished == null && x.User == user);

                if(absItem != null)
                    absItemType = absItem.AbsItemType;
            }

            if (absItemType == null)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Вы не воздерживаетесь в текущий момент. Зайдите в бота и начните свое воздержание.", replyToMessageId: update.Message.MessageId);
                return;
            }
            
            var span = absItemDbRepository.GetCurrentTerm(user, absItemType.Value);

            if (span != null)
            {
                var str = String.Format ("<b>{0:00}</b> " + Speller.DaysString((int) span?.Days) + ", <b>{1:00}</b> " + Speller.HoursString((int) span?.Hours) + ", <b>{2:00}</b> " + Speller.MinutesString((int) span?.Minutes) + ", <b>{3:00}</b> " + Speller.SecondsString((int) span?.Seconds), span?.Days, span?.Hours, span?.Minutes, span?.Seconds);

                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    $"Текущий срок воздержания \"{absItemType}\":\n\n" + str, ParseMode.Html, replyToMessageId: update.Message.MessageId);
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(chatId,
                    "Произошла ошибка", ParseMode.Html);
            }
        }
    }
}