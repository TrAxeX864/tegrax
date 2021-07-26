using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;
using User = TgVozderzhansBot.Models.User;

namespace TgVozderzhansBot.Core.Workers
{
    public class PollWorker
    {
        private readonly TelegramBotClient _botClient;

        public PollWorker(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        
        public void Start()
        {
            new Thread(async () =>
            {
                Console.WriteLine("PollWorker has been started.");
                
                while (true)
                {
                    Thread.Sleep(5000);

                    await using ApplicationContext db = new ApplicationContext();

                    UserDbRepository userDbRepository = new UserDbRepository();
                    
                    var pollList = db.PollItems.Include(x => x.Owner)
                        .Where(x => x.FinishedAt == null).ToList();

                    foreach (var poll in pollList)
                    {
                        var allUsers = userDbRepository.GetAllUsers();

                        while (poll.Position < poll.TotalUsers)
                        {
                            try
                            {
                                User currentUser = allUsers[poll.Position];
                            
                                poll.Position = poll.Position + 1;

                                await _botClient.SendPollAsync(currentUser.ChatId, poll.Question, poll.GetOptions(),
                                    isAnonymous: false);

                                db.PollItems.Update(poll);
                                await db.SaveChangesAsync();

                                Thread.Sleep(300);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + " " + ex.StackTrace);
                            }
                        }
                        
                        poll.FinishedAt = DateTime.Now;
                        db.PollItems.Update(poll);
                        
                        await db.SaveChangesAsync();

                        await _botClient.SendTextMessageAsync(poll.Owner.ChatId, $"Рассылка голосования с ID: {poll.Id} завершена.");
                    }
                }
            }).Start();
        }
    }
}