using System;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Message)]
    public class InputChangeAbsCommandHandler : CommandHandler
    {
        public InputChangeAbsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message != null)
            {
                using ApplicationContext db = new ApplicationContext();
                
                UserDbRepository userDbRepository = new UserDbRepository();

                var user = userDbRepository.GetUser(update.Message.Chat.Id);

                var changeAbsWaitItemCount = db.ChangeAbsWaitItems.Count(x => x.User == user);

                if (changeAbsWaitItemCount > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            using ApplicationContext db = new ApplicationContext();
                
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            var changeAbsWaitItem = db.ChangeAbsWaitItems.FirstOrDefault(x => x.User == user);

            string text = update.Message.Text;

            bool result = int.TryParse(text?.Trim(), out int num);

            if (result)
            {
                var absItem = db.AbsItems.FirstOrDefault(x =>
                    x.Finished == null && x.User == user && x.AbsItemType == changeAbsWaitItem.AbsItemType);

                if (absItem != null)
                {
                    absItem.Started = DateTime.Now.AddDays(-num);

                    db.AbsItems.Update(absItem);
                    
                    if(changeAbsWaitItem != null)
                        db.ChangeAbsWaitItems.Remove(changeAbsWaitItem);
                    
                    db.SaveChanges();

                    await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                        "Вы успешно поменяли свой срок воздержания.");
                }
            }
            else
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Срок воздержания введен неверно. Нажмите кнопку <b>\"Поменять срок воздержания\"</b> еще раз.", ParseMode.Html);

                if (changeAbsWaitItem != null)
                {
                    db.ChangeAbsWaitItems.Remove(changeAbsWaitItem);
                    db.SaveChanges();
                }
            }
        }
    }
}