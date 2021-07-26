using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class RefPremiumCommandHandler : CommandHandler
    {
        public RefPremiumCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data == "ref_premium")
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            string text = "Чтобы получить <b>от 3 до 10 дней</b> премиум аккаунта бесплтано, " +
                          "Вам нужно пригласить всего-лишь одного друга. Необходимо, чтобы Ваш друг перешел по ссылке: " +
                          $"https://t.me/Vozderzhans13_Bot?start={update.CallbackQuery.Message.Chat.Id}";

            await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, ParseMode.Html);
            await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
        }
    }
}