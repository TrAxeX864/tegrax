using System.Linq;
using System.Text.RegularExpressions;
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
    public class CreatePollCommandHandler : CommandHandler
    {
        public CreatePollCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text.StartsWith("/create_poll"))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();
            
            Regex regex = new Regex("/create_poll\\s(.*?)\\s\\{");
            Regex regex2 = new Regex("\\{(.*?)\\}");
            
            var matches = regex2.Matches(update.Message.Text);
            var match = regex.Match(update.Message.Text);

            if (match.Success && matches.Count > 0)
            {
                string question = match.Groups[1].Value;

                string[] options = matches.Select(x => x.Groups[1].Value).ToArray();

                var user = userDbRepository.GetUser(update.Message.Chat.Id);
            
                using ApplicationContext db = new ApplicationContext();
            
                PollItem pollItem = new PollItem();
                pollItem.Position = 0;
                pollItem.Owner = db.Users.FirstOrDefault(x => x.Id == user.Id);
                pollItem.Question = question;
                pollItem.SetOptions(options);
                pollItem.TotalUsers = userDbRepository.GetUsersCount();

                await db.PollItems.AddAsync(pollItem);
                await db.SaveChangesAsync();

                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    $"Рассылка голосования с ID: {pollItem.Id} создана.");
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Запрос составлен неправильно.");
            }
        }
    }
}