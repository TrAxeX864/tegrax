using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.Workers
{
    public class AbsGuardWorker
    {
        private readonly TelegramBotClient _botClient;
        
        public AbsGuardWorker(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        
        public void Start()
        {
            Console.WriteLine("AdsGuardWorker has been started");
            
            new Thread(async () =>
            {
                while (true)
                {
                    Thread.Sleep(5000);

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();
                        UserDbRepository userDbRepository = new UserDbRepository();
                        AbsGuardItemDbRepository absGuardItemDbRepository = new AbsGuardItemDbRepository();
                        
                        var users = db.Users.Include(x => x.AbsItems).Where(x => 
                            !(x.PremiumActiveTo != null && DateTime.Now <= x.PremiumActiveTo) &&
                            x.AbsItems.Count(c => c.Finished == null) > 0 &&
                            x.PublicAccount).ToList();

                        foreach (var user in users)
                        {
                            foreach (var absItem in user.AbsItems.Where(x => x.Finished == null).ToList())
                            {
                                var absGuardItem = db.AbsGuardItems.FirstOrDefault(x => x.User == user && x.AbsItem.AbsItemType == absItem.AbsItemType && x.LastNotify < DateTime.Now.AddDays(-1));

                                bool isConfirmed = db.AbsGuardItems.Count(x =>
                                    x.User == user && x.AbsItem == absItem && (x.ConfirmDate != null && x.ConfirmDate > DateTime.Now.AddDays(-14))) > 0;
                                
                                long chatId = user.ChatId;

                                if (absGuardItem == null && !isConfirmed)
                                {
                                    var absGuardItem2 =
                                        db.AbsGuardItems.FirstOrDefault(x => x.User == user && x.AbsItem.AbsItemType == absItem.AbsItemType);

                                    if (absGuardItem2 == null || !absGuardItem2.Flag2)
                                    {
                                        var buttons = new List<InlineKeyboardButton>();
                            
                                        buttons.Add(new InlineKeyboardButton
                                        {
                                            Text = "Подтвердить воздержание",
                                            CallbackData = $"confirm_abs:{absItem.AbsItemType}"
                                        });

                                        try
                                        {
                                            await _botClient.SendTextMessageAsync(chatId, $"Подтвердите свое воздержание по категории <b>\"{absItem.AbsItemType}\"</b>. " +
                                                $"В противном случае ваш аккаунт будет переведен в приватный режим.", ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                                        }
                                        catch(Exception ex)
                                        {
                                            Console.WriteLine(ex.StackTrace + " " + ex.Message);
                                        }
                                        
                                        absGuardItemDbRepository.MakeNotify(user, absItem.AbsItemType);
                                    
                                        Thread.Sleep(500);

                                        if (absGuardItem2 == null)
                                        {
                                            absGuardItem2 =
                                                db.AbsGuardItems.FirstOrDefault(x => x.User == user && x.AbsItem.AbsItemType == absItem.AbsItemType);

                                            if (absGuardItem2 != null)
                                            {
                                                absGuardItem2.Flag2 = true;
                                                db.AbsGuardItems.Update(absGuardItem2);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (absGuardItem != null)
                                    {
                                        if (!absGuardItem.Flag)
                                        {
                                            absGuardItemDbRepository.MakeNotify(user, absItem.AbsItemType);

                                            absGuardItem.Flag = true;

                                            db.AbsGuardItems.Update(absGuardItem);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (absGuardItem.Flag && absGuardItem.Flag2 && !isConfirmed)
                                                {
                                                    db.AbsGuardItems.Remove(absGuardItem);
                                                    db.SaveChanges();

                                                    if (absGuardItem.ConfirmDate != null)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                
                                            }

                                            if (!isConfirmed)
                                            {
                                                userDbRepository.SetAccountPrivacy(user, false);

                                                try
                                                {
                                                    await _botClient.SendTextMessageAsync(chatId,
                                                        "Вы не подтвердили свое воздержание, вашему аккаунту присвоен статус \"приватный\". Вы не будете отображаться в топе воздержания. Статус аккаунта вы можете поменять в настройках.");
                                                }
                                                catch(Exception ex)
                                                {
                                                    Console.WriteLine(ex.StackTrace + " " + ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }).Start();
        }
    }
}