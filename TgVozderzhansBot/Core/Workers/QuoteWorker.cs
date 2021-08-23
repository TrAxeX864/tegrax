using System;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Migrations;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.Workers
{
    public class QuoteWorker
    {
        private TelegramBotClient _botClient;
        
        public QuoteWorker(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        
        public void Start()
        {
            new Thread(async () =>
            {
                await using ApplicationContext db = new ApplicationContext();
                UserDbRepository userDbRepository = new UserDbRepository();

                while (true)
                {
                    var users = userDbRepository.GetAllUsers();

                    var rand = new Random();
                
                    foreach (var user in users)
                    {
                        int attempts = 0;
                    
                        m1:

                        var skip = (int)(rand.NextDouble() * db.Quotes.Count());
                        var quote = db.Quotes.OrderBy(x => x.Id).Skip(skip).Take(1).FirstOrDefault();

                        int cc = db.QuoteTables.Count(x => x.User == user && x.UpdatedAt > DateTime.Now.AddDays(-1));
                    
                        if (quote != null && cc == 0)
                        {
                            int c = db.QuoteTables.Count(x => x.User == user && x.LastQuote == quote);

                            if (c == 0)
                            {
                                var quoteTable = new QuoteTable();
                                quoteTable.User = db.Users.FirstOrDefault(x => x.Id == user.Id);
                                quoteTable.LastQuote = quote;
                                quoteTable.UpdatedAt = DateTime.Now;
                            
                                await db.QuoteTables.AddAsync(quoteTable);
                                await db.SaveChangesAsync();

                                await _botClient.SendTextMessageAsync(user.ChatId, "<b>Цитата дня</b>\n\n" + quote.Text + $"\n\n<i>{quote.Name}</i>", ParseMode.Html);
                            }
                            else
                            {
                                attempts++;
                            
                                if(attempts < 5)
                                    goto m1;
                            }                        
                        }
                    }   
                    
                    Thread.Sleep(15000);
                }
                
            }).Start();
        }
    }
}