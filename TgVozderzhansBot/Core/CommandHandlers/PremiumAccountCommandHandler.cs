using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Message)]
    public class PremiumAccountCommandHandler : CommandHandler
    {
        public PremiumAccountCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text.EndsWith("Премиум Аккаунт"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            var timeSpan= userDbRepository.GetPremiumRemaining(user);

            string str = "";
            
            if (timeSpan != null)
            {
                str = $"<b>Текущий статус премиум аккаунта:</b> {timeSpan.Value.TotalDays:F0} {Speller.DaysString((int)timeSpan.Value.TotalDays)}\n\n";
            }
            
            string text = $"{str}Премиум аккаунт дает дополнительные возможности.\n\n" +
                          "<b>1.</b> В топе пользователей ваш аккаунт подсвечен звездой. Вы выделяетесь среди остальных пользователей.\n\n" +
                          "<b>2.</b> Есть возможность менять срок воздержания. Возможно выставлять любое количество дней.\n\n" +
                          "<b>3.</b> Не показывается табличка подтверждения воздержания. Нет надобности подтверждать воздержание каждые 2 недели.\n\n" +
                          "<b>4.</b> Возможность добавлять более двух друзей.\n\n" +
                          "<b>5.</b> Возможность просмотра истории воздержания.\n\n" +
                          "<b>6.</b> Возможность воздерживаться более чем по одной категории.\n\n" +
                          "<b>Приобретение премиум аккаунта:</b>\n" +
                          "Чтобы приобрести премиум аккаунт на месяц, Вам нужно скинуть 100 руб. на карту <b>\"2202 2006 1035 7640\"</b> через Сбербанк Онлайн. В комментариях указать свой никнейм в Telegram (который с собакой). А затем написать администратору @napsy13.";

            var button = new InlineKeyboardButton
            {
                Text = "Получить премиум бесплатно",
                CallbackData = "ref_premium"
            };
            
            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, text, ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(button));
        }
    }
}