using System;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class ConfirmAbsCommandHandler : CommandHandler
    {
        public ConfirmAbsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("confirm_abs:"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var absItemType = Enum.Parse<AbsItemType>(update.CallbackQuery.Data.Split(":")[1]);

            UserDbRepository userDbRepository = new UserDbRepository();
            AbsGuardItemDbRepository absGuardItemDbRepository = new AbsGuardItemDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);
            
            if(user.PublicAccount)
                absGuardItemDbRepository.ConfirmAbs(user, absItemType);

            await TelegramBotClient.DeleteMessageAsync(new ChatId(update.CallbackQuery.Message.Chat.Id), update.CallbackQuery.Message.MessageId);
        }
    }
}