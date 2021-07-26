using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Core.Workers
{
    /**
     * Воркер рассылки сообщений пользователям в Telegram
     */
    public class MailingWorker
    {
        private readonly TelegramBotClient _botClient; 
        
        public MailingWorker(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        
        public void Start()
        {
            new Thread(async () =>
            {
                Console.WriteLine("MailingWorker has been started.");
                
                while (true)
                {
                    Thread.Sleep(5000);

                    await using ApplicationContext db = new ApplicationContext();

                    UserDbRepository userDbRepository = new UserDbRepository();
                    
                    var mailingList = db.MailingItems.Include(x => x.Owner)
                        .Where(x => x.FinishedAt == null).ToList();

                    foreach (var mailing in mailingList)
                    {
                        var allUsers = userDbRepository.GetAllUsers();

                        while (mailing.Position < mailing.TotalUsers)
                        {
                            try
                            {
                                User currentUser = allUsers[mailing.Position];
                            
                                mailing.Position = mailing.Position + 1;

                                db.MailingItems.Update(mailing);
                                await db.SaveChangesAsync();

                                await _botClient.SendTextMessageAsync(currentUser.ChatId, mailing.Message, ParseMode.Html);

                                Thread.Sleep(300);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + " " + ex.StackTrace);
                            }
                        }
                        
                        mailing.FinishedAt = DateTime.Now;
                        db.MailingItems.Update(mailing);
                        
                        await db.SaveChangesAsync();

                        await _botClient.SendTextMessageAsync(mailing.Owner.ChatId, $"Рассылка с ID: {mailing.Id} завершена.");
                    }
                }
            }).Start();
        }
    }
}