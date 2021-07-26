using System;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class ChangeAbsTermCommandHandler : CommandHandler
    {
        public ChangeAbsTermCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("change_abs_term:"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            AbsItemType absItemType = Enum.Parse<AbsItemType>(update.CallbackQuery.Data.Split(":")[1]);
            
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            if (!user.HasPremium)
            {
                await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id,
                    "Для работы данной функции нужен премиум аккаунт.", true);
                return;
            }
            
            string caption = "Введите новый срок воздержания <b>в днях</b>. Нужно ввести просто число:";

            await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, caption, ParseMode.Html);

            using (ApplicationContext db = new ApplicationContext())
            {
                db.ChangeAbsWaitItems.RemoveRange(db.ChangeAbsWaitItems.Where(x => x.User == user));
                
                var changeAbsWaitItem = new ChangeAbsWaitItem();
                changeAbsWaitItem.User = db.Users.FirstOrDefault(x => x.Id == user.Id);
                changeAbsWaitItem.AbsItemType = absItemType;

                db.ChangeAbsWaitItems.Add(changeAbsWaitItem);
                db.SaveChanges();
            }
            
            await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
        }
    }
}