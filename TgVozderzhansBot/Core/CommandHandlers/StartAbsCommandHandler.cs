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
    public class StartAbsCommandHandler : CommandHandler
    {
        public StartAbsCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery != null && update.CallbackQuery.Data.StartsWith("startabs_"))
            {
                return true;
            }

            return false;
        }

        public override async Task Handler(Update update)
        {
            AbsItemType absItemType = Enum.Parse<AbsItemType>(update.CallbackQuery.Data.Split("_")[1]);
            
            UserDbRepository userDbRepository = new UserDbRepository();
            AbsItemDbRepository absItemDbRepository = new AbsItemDbRepository();

            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            var currentAbstinenceList = absItemDbRepository.GetCurrentAbstinenceList(user);

            if (!user.HasPremium && currentAbstinenceList.Count > 0)
            {
                await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id,
                    "Чтобы воздерживаться более чем по одной категории, вам нужен премиум аккаунт", true);
                return;
            }
            
            bool flag = absItemDbRepository.StartNewAbstinence(user, absItemType);
            
            // Выдаем премиум пригласившему нас другу
            using ApplicationContext db = new ApplicationContext();

            if (db.Users.Count(x => x.Id == user.Id && !x.IsFriendGotPremium) > 0)
            {
                Random rnd = new Random();
                int premiumDays = rnd.Next(3, 11);

                var user2 = db.Users.FirstOrDefault(x => x.Id == user.Id);

                if (user2 != null && user2.InviterChatId != user2.ChatId)
                {
                    if(user2.PremiumActiveTo == null) user2.PremiumActiveTo = DateTime.Now;

                    user2.IsFriendGotPremium = true;
                    user2.PremiumActiveTo = user2.PremiumActiveTo.Value.AddDays(premiumDays);

                    db.Users.Update(user2);
                    await db.SaveChangesAsync();

                    try
                    {
                        if (user2.InviterChatId != null)
                        {
                            await TelegramBotClient.SendTextMessageAsync(user2.InviterChatId,
                                $"Вы получили премиум на <b>{premiumDays} {Speller.DaysString(premiumDays)}</b> за приглашенного пользователя @{user2.Username}.",
                                ParseMode.Html);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " " + ex.StackTrace);
                    }
               }
            }
            ///////
            
            string text = null;

            if (flag)
            {
                text =
                    $"Воздержание <b>\"{absItemType}\"</b> успешно начато! Проверять свой срок воздержания Вы можете нажав по кнопке <b>\"Мое воздержание\"</b>";
            }
            else
            {
                text = "Возникла ошибка, не удалось начать воздержание!";
            }

            await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, ParseMode.Html);

            try
            {
                await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
            }
            catch
            {
                
            }
        }
    }
}