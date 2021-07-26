using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class SetTermCommandHandler : CommandHandler
    {
        public SetTermCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text != null && update.Message.Text.StartsWith("/set_term"))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var splitData = update.Message.Text.Split(" ");

            try
            {
                string nickname = splitData[1];
                AbsItemType category = Enum.Parse<AbsItemType>(splitData[2]);
                bool result = int.TryParse(splitData[3], out int days);

                if (result)
                {
                    var user = userDbRepository.GetUserByNickname(nickname);
                    var absItem = user.AbsItems.FirstOrDefault(x => x.AbsItemType == category && x.Finished == null);

                    if (absItem != null)
                    {
                        absItem.Started = DateTime.Now.AddDays(-days);
                        
                        using ApplicationContext db = new ApplicationContext();
                        
                        db.AbsItems.Update(absItem);
                        db.SaveChanges();
                        
                        await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Команда выполнена успешно.");
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Ошибка при обработке команды.");
            }
        }
    }
}