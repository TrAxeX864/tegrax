using System;
using System.Reflection;
using System.Threading;
using TegraX.Core;
using Telegram.Bot;

namespace TegraXTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramBotClient botClient = new TelegramBotClient("570569776:AAEdSvSRPAfzQZ1CbbVwmu1OWiMKTwsnTDc");
            
            TegraBot tegraBot = new TegraBot(botClient);
          
            var types = Assembly.GetExecutingAssembly().GetTypes();

            tegraBot.Start(types);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}