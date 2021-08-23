using System;
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
    public class AddQuoteCommandHandler : CommandHandler
    {
        public AddQuoteCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();

            var user = userDbRepository.GetUser(update.Message.Chat.Id);

            if (user.IsAdmin)
            {
                if (update.Message.Text.StartsWith("/add_quote "))
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            var splitData = update.Message.Text.Split("/add_quote ");

            string data = splitData[1];

            string name = null;
            
            var match = Regex.Match(data, "\\|(.*?)\\|");

            if (match.Success)
            {
                name = match.Groups[1].Value;
            }
            
            data = Regex.Replace(data, "\\|(.*?)\\|", "");

            data = data.Trim();

            try
            {

                if (data.Length > 5)
                {
                    await using ApplicationContext db = new ApplicationContext();

                    var quote = new Quote();
                    quote.Name = name;
                    quote.Text = data;
                    quote.CreatedAt = DateTime.Now;
                
                    await db.Quotes.AddAsync(quote);
                    await db.SaveChangesAsync();

                    await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Цитата успешно добавлена!");
                }
                else
                {
                    await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Не удалось добавить цитату");
                }
            }
            catch
            {
                await TelegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Не удалось добавить цитату");
            }
        }
    }
}