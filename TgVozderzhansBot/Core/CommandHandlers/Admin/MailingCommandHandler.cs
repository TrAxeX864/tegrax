using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class MailingCommandHandler : CommandHandler
    {
        public MailingCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text.StartsWith("/mailing "))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            string message = update.Message.Text.Replace("/mailing ", "");
            
            UserDbRepository userDbRepository = new UserDbRepository();
            
            using ApplicationContext db = new ApplicationContext();
            
            MailingItem item = new MailingItem();
            item.Owner = db.Users.FirstOrDefault(x => x.ChatId == update.Message.Chat.Id);
            item.Message = message;
            item.Position = 0;
            item.TotalUsers = userDbRepository.GetUsersCount();

            await db.MailingItems.AddAsync(item);
            await db.SaveChangesAsync();

            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                $"Рассылка с ID: {item.Id} добавлена. Когда будет завершена рассылка, вам придет сообщение.");
        }
    }
}