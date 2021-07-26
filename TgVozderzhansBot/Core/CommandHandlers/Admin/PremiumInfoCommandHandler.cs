using System;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class PremiumInfoCommandHandler : CommandHandler
    {
        public PremiumInfoCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text.StartsWith("/premium_info"))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.Message.Text.Split(" ");

            string username = splitData[1];
            
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUserByNickname(username);

            if (user == null)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Пользователь не найден.");
                return;
            }

            TimeSpan? premiumRemaining = userDbRepository.GetPremiumRemaining(user);

            if (premiumRemaining == null)
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "У данного пользователя нет премиум аккаунта.");
                return;
            }

            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Осталось дней премиум аккаунта: " + premiumRemaining.Value.TotalDays.ToString("F0"));
        }
    }
}