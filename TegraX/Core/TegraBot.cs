using System;
using System.Collections.Generic;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#pragma warning disable 618

namespace TegraX.Core
{
    public class TegraBot
    {
        private readonly TelegramBotClient _client;
        
        private readonly List<CommandHandler> _commandHandlers = new List<CommandHandler>();

        public delegate bool BeforeHandle(Update update);
        public event BeforeHandle OnBeforeHandle;
        
        public TegraBot(TelegramBotClient client)
        {
            _client = client;
        }

        private void CreateCommandHandlers(Type[] assemblyTypes)
        {
            foreach (var type in assemblyTypes)
            {
                if (type.IsClass)
                {
                    if (type.IsSubclassOf(typeof(CommandHandler)))
                    {
                        Console.WriteLine(type.FullName);

                        _commandHandlers.Add(Activator.CreateInstance(type, _client) as CommandHandler);
                    }
                }
            }
        }

        private void UpdateAction()
        {
            _client.OnUpdate += async (sender, update) =>
            {
                try
                {
                    foreach (var commandHandler in _commandHandlers)
                    {
                        bool? result = OnBeforeHandle?.Invoke(update.Update);

                        if (result != null)
                        {
                            if (result.Value)
                            {
                                if (commandHandler.SupportedType == CmdType.Callback && update.Update.Type == UpdateType.CallbackQuery && commandHandler.AllowNext(update.Update))
                                {
                                    if (await commandHandler.CanHandle(update.Update))
                                    {
                                        await commandHandler.Handler(update.Update);
                                    }
                                } 
                                else if (commandHandler.SupportedType == CmdType.Message && update.Update.Type == UpdateType.Message && commandHandler.AllowNext(update.Update))
                                {
                                    if (update.Update.Message?.Text != null && await commandHandler.CanHandle(update.Update))
                                    {
                                        await commandHandler.Handler(update.Update);
                                    }
                                } 
                                else if (commandHandler.SupportedType == CmdType.All && commandHandler.AllowNext(update.Update))
                                {
                                    if (await commandHandler.CanHandle(update.Update))
                                    {
                                        await commandHandler.Handler(update.Update);
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (commandHandler.SupportedType == CmdType.Callback && update.Update.Type == UpdateType.CallbackQuery && commandHandler.AllowNext(update.Update))
                            {
                                if (await commandHandler.CanHandle(update.Update))
                                {
                                    await commandHandler.Handler(update.Update);
                                }
                            } 
                            else if (commandHandler.SupportedType == CmdType.Message && update.Update.Type == UpdateType.Message && commandHandler.AllowNext(update.Update))
                            {
                                if (update.Update.Message?.Text != null && await commandHandler.CanHandle(update.Update))
                                {
                                    await commandHandler.Handler(update.Update);
                                }
                            }
                            else if (commandHandler.SupportedType == CmdType.All && commandHandler.AllowNext(update.Update))
                            {
                                if (await commandHandler.CanHandle(update.Update))
                                {
                                    await commandHandler.Handler(update.Update);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " " + ex.StackTrace);
                }
            };
        }
        
        public void Start(Type[] assemblyTypes)
        {
            this.CreateCommandHandlers(assemblyTypes);
            
            this.UpdateAction();
            
            _client.StartReceiving();
        }
    }
}