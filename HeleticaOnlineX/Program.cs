using System;
using System.Reflection;
using HeleticaOnlineX.Core;
using TegraX.Core;
using Telegram.Bot;

namespace HeleticaOnlineX
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigRepository configRepository = new ConfigRepository();

            string botToken = configRepository.ReadTgBotToken();

            TelegramBotClient botClient = new TelegramBotClient(botToken);
            
            TegraBot tegraBot = new TegraBot(botClient);

            var types = Assembly.GetExecutingAssembly().GetTypes();

            tegraBot.Start(types);
        }
    }
}