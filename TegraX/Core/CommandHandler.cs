using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TegraX.Core
{
    public abstract class CommandHandler
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class HandleOnlyFromGroup : Attribute
        {
        
        }
        
        protected TelegramBotClient TelegramBotClient;

        public CmdType? SupportedType { get; set; }
        
        public CommandHandler(TelegramBotClient bot)
        {
            TelegramBotClient = bot;
            
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(this.GetType());  // Reflection.  

            SupportedType = attrs.Select(x => (x as CommandType)?.Type).FirstOrDefault();
        }
        
        public bool AllowNext(Update update)
        {
            ChatType? chatType = update?.CallbackQuery?.Message?.Chat?.Type ?? update?.Message?.Chat?.Type;

            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(this.GetType());  // Reflection.  

            var count = attrs.Where(x => x is HandleOnlyFromGroup).Count();

            if (count > 0)
            {
                if (chatType != ChatType.Supergroup) return false;
                else return true;
            }

            if (chatType == ChatType.Supergroup)
            {
                return false;
            }
            
            if (SupportedType == null)
            {
                return false;
            }

            return true;
        }
        
        public abstract Task<bool> CanHandle(Update update);

        public abstract Task Handler(Update update);
    }
}