using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers
{
    [CommandType(CmdType.Callback)]
    public class ChangeAccountPrivacyCommandHandler : CommandHandler
    {
        public ChangeAccountPrivacyCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data == "set_private_account")
            {
                return true;
            }

            if (update.CallbackQuery.Data == "set_public_account")
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            if (update.CallbackQuery.Data == "set_public_account")
            {
                userDbRepository.SetAccountPrivacy(user, true);
            }

            if (update.CallbackQuery.Data == "set_private_account")
            {
                userDbRepository.SetAccountPrivacy(user, false);
            }
            
            SettingsCommandHandler settingsCommandHandler = new SettingsCommandHandler(TelegramBotClient);

            settingsCommandHandler.EnableUpdateFlag();
            settingsCommandHandler.Handler(update);
        }
    }
}