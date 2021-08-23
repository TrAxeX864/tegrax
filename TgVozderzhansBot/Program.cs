using System;
using System.Reflection;
using System.Threading;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TgVozderzhansBot.Core;
using TgVozderzhansBot.Core.Workers;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot
{
    class Program
    {
        static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            TelegramBotClient botClient = new TelegramBotClient(DotNetEnv.Env.GetString("TG_BOT_TOKEN"));
            
            TegraBot tegraBot = new TegraBot(botClient);

            tegraBot.OnBeforeHandle += update =>
            {
                long? chatId = null;

                if (update.Type == UpdateType.CallbackQuery) chatId = update.CallbackQuery.Message.Chat.Id;
                if (update.Type == UpdateType.Message) chatId = update.Message.Chat.Id;

                if (chatId != null)
                {
                    UserDbRepository userDbRepository = new UserDbRepository();

                    // Обработка бана юзера
                    var user = userDbRepository.GetUser(chatId.Value);

                    if (user != null && (user.Username == null || user.Username.Length < 2))
                    {
                        botClient.SendTextMessageAsync(chatId,
                            "Чтобы пользоваться ботом, вам нужно выставить себе никнейм с собачкой. Это делается в настройках Telegram.");

                        return false;
                    }
                    
                    if (user != null && user.IsBanned)
                    {
                        botClient.SendTextMessageAsync(chatId,
                            "Вы были забанены. Пожалуйста, обратитесь к администратору @napsy13");
                        
                        return false;
                    }
                }
                
                return true;
            };
          
            var types = Assembly.GetExecutingAssembly().GetTypes();

            tegraBot.Start(types);

            // Запускаем воркеры
            var adsGuardWorker = new AbsGuardWorker(botClient);
            adsGuardWorker.Start();

            var mailingWorker = new MailingWorker(botClient);
            mailingWorker.Start();
            
            var pollWorker = new PollWorker(botClient);
            pollWorker.Start();
            
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}