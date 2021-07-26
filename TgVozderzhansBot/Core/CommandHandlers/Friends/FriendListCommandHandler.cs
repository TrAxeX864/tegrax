using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegraX.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgVozderzhansBot.Database.Repositories;

namespace TgVozderzhansBot.Core.CommandHandlers.Friends
{
    [CommandType(CmdType.Callback)]
    public class FriendListCommandHandler : CommandHandler
    {
        public FriendListCommandHandler(TelegramBotClient bot) : base(bot)
        {
        }

        public override async Task<bool> CanHandle(Update update)
        {
            if (update.CallbackQuery.Data.StartsWith("friend-list"))
            {
                return true;
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.StartsWith("friend-list-nav"))
                {
                    return true;
                }
            }
            
            return false;
        }

        private const int ItemsOnPage = 10;

        public override async Task Handler(Update update)
        {
            UserDbRepository userDbRepository = new UserDbRepository();
            FriendDbRepository friendDbRepository = new FriendDbRepository();
            
            int offset = 0;
            
            var user = userDbRepository.GetUser(update.CallbackQuery.Message.Chat.Id);

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.Contains("nav"))
                {
                    offset = int.Parse(update.CallbackQuery.Data.Split(":").Last());
                    
                    if (offset < 0) offset = 0;
                }
            }
            
            var friendList = friendDbRepository.GetMyFriendsList(user, offset, ItemsOnPage);

            int totalFriendsCount = friendDbRepository.GetMyFriendListCount(user);

            string text = "Список ваших друзей:\n";

            if (friendList == null || friendList.Count == 0)
            {
                text += "\n<i>Не найдено ни одного друга.</i>";

                if (update.CallbackQuery.Data.Contains("back"))
                {
                    await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId,
                        text, ParseMode.Html);
                }
                else
                {
                    await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, ParseMode.Html);
                }
                
                await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
            }
            else
            {
                var buttons = new List<List<InlineKeyboardButton>>();
                var navButtons = new List<InlineKeyboardButton>();
                
                if (offset + 1 > ItemsOnPage)
                {
                    int newOffset = offset - ItemsOnPage;
                
                    if (newOffset < 0) newOffset = 0;

                    navButtons.Add(new InlineKeyboardButton
                    {
                        Text = "<<",
                        CallbackData = $"friend-list-nav:{newOffset}"
                    });
                }
                
                if (ItemsOnPage+offset < totalFriendsCount)
                {
                    navButtons.Add(new InlineKeyboardButton
                    {
                        Text = ">>",
                        CallbackData = $"friend-list-nav:{(offset+ItemsOnPage)}"
                    });
                }
                
                foreach (var friendItem in friendList)
                {
                    buttons.Add(new List<InlineKeyboardButton> {
                        new InlineKeyboardButton
                        {
                            Text = friendItem.Friend.Username,
                            CallbackData = $"navfriend:{friendItem.Id}"
                        }
                    });
                }
                
                // if (!user.HasPremium)
                // {
                //     buttons = buttons.Take(2).ToList();
                //     navButtons = new List<InlineKeyboardButton>();
                //
                //     text = "<b>Внимание!</b> У вас нет премиум аккаунта. Будет отображаться только два друга.\n\n" + text;
                // }
                
                buttons.Add(navButtons);

                if (update.Type == UpdateType.CallbackQuery && 
                    (update.CallbackQuery.Data.StartsWith("friend-list-nav") || update.CallbackQuery.Data.Contains("back")))
                {
                    try
                    { 
                        await TelegramBotClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                            update.CallbackQuery.Message.MessageId,
                            text, ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    }
                    catch
                    {

                    }
                    finally
                    {
                        await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                    }
                
                }
                else
                {
                    await TelegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, text, 
                        replyMarkup: new InlineKeyboardMarkup(buttons), parseMode: ParseMode.Html);
                    
                    await TelegramBotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                }
            }
        }
    }
}