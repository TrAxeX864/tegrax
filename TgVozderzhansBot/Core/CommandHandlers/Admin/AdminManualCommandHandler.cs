using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgVozderzhansBot.Core.CommandHandlers.Admin
{
    [CommandType(CmdType.Message)]
    public class AdminManualCommandHandler : CommandHandler
    {
        public AdminManualCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.Message.Text == "/admin")
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            string manual = "<b>1.</b> Команда бана пользователя. <b>Синтаксис:</b> <i>/ban_user [nickname]</i>.\n" +
                            "<b>2.</b> Команда разбана пользователя. <b>Синтаксис:</b> <i>/unban_user [nickname]</i>.\n" +
                            "<b>3.</b> Команда установки срока воздержания пользователю. <b>Синтаксис:</b> <i>/set_term [nickname] [category] [days]</i>\n" +
                            "<b>4.</b> Команда добавления дней премиума пользователю. <b>Синтаксис:</b> <i>/add_premium [nickname] [days].</i>\n" +
                            "<b>5.</b> Команда для удаления премиума пользователю. <b>Синтаксис</b> <i>/delete_premium [nickname].</i>\n" +
                            "<b>6.</b> Команда рассылки сообщений пользователям. <b>Синтаксис:</b> <i>/mailing [message].</i>\n" +
                            "<b>7.</b> Команда для проверки текущего количества пользователей в системе. <b>Синтаксис:</b> <i>/users_count</i>.\n" +
                            "<b>8.</b> Команда для установки приватности аккаунта пользователю. <b>Синтаксис:</b> <i>/set_privacy [username] [true|false].</i>\n" +
                            "<b>9.</b> Команда для проверки срока премиума пользователя. <b>Синтаксис:</b> <i>/premium_info [username].</i>.\n" +
                            "<b>10.</b> Команда для создания голосования в боте. <b>Синтаксис:</b> <i>/create_poll [question] {option1} {option2} {option3} ...</i>";
                          
            await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, manual, ParseMode.Html);
        }
    }
}