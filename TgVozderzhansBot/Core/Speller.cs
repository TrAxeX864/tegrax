using System.Globalization;
using System.Linq;

namespace TgVozderzhansBot.Core
{
    public class Speller
    {
        public static string DaysString(int number)
        {
            var titles = new[] {"день", "дня", "дней"};
            var cases = new[] {2, 0, 1, 1, 1, 2};
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
        }

        public static string HoursString(int number)
        {
            var titles = new[] {"час", "часа", "часов"};
            var cases = new[] {2, 0, 1, 1, 1, 2};
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
        }
        
        public static string MinutesString(int number)
        {
            var titles = new[] {"минута", "минуты", "минут"};
            var cases = new[] {2, 0, 1, 1, 1, 2};
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
        }
        
        public static string SecondsString(int number)
        {
            var titles = new[] {"секунда", "секунды", "секунд"};
            var cases = new[] {2, 0, 1, 1, 1, 2};
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
        }
    }
}