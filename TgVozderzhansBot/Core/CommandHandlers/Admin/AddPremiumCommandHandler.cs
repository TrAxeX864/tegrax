using System;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class AddPremiumCommandHandler : CommandHandler
    {
        public AddPremiumCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text.StartsWith("/add_premium"))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            try
            {
                UserDbRepository userDbRepository = new UserDbRepository();

                var splitData = update.Message.Text.Split(" ");

                string userNickname = splitData[1];
                int days = int.Parse(splitData[2]);

                var user = userDbRepository.GetUserByNickname(userNickname);

                if (user != null)
                {
                    using ApplicationContext db = new ApplicationContext();
                    
                    if(user.PremiumActiveTo == null) user.PremiumActiveTo = DateTime.Now;

                    user.PremiumActiveTo = user.PremiumActiveTo.Value.AddDays(days);

                    db.Users.Update(user);
                    db.SaveChanges();
                    
                    await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Команда успешно обработана.");
                }
            }
            catch
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "При вводе команды возникла ошибка.");
            }
        }
    }
}