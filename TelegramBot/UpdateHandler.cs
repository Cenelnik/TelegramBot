using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.ApiArduino.Interfaces;
using TelegramBot.ApiArduino.Classes;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot
{
    internal  class UpdateHandler : IUpdateHandler
    {
        private  delegate void Logger(Message msg, string st);
        private IArduinoExecutable Command;
        private string responsToClient = "";

        
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MSG = {exception.Message}; \nTRACE = {exception.StackTrace}");
            Task.CompletedTask.Wait(cancellationToken);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            try
            {
                if(!cancellationToken.IsCancellationRequested)
                {
                    await Task.Run(() => MainWorker(update, botClient));
                }else
                {
                    Console.WriteLine($"cancellationToken.IsCancellationRequested = {cancellationToken.IsCancellationRequested} => Task was stoped.");
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG = {ex.Message}; \nTRACE = {ex.StackTrace}");
            }
        }

        private  async void MainWorker(Update update, ITelegramBotClient botClient)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddHttpClient();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            switch (update.Type)
            {
                case UpdateType.Message:
                    
                    var message = update.Message;
                    User ?user = message.From;

                    Logger log = LogBot;
                    log(update.Message, "");

                    switch(message.Text)
                    {
                        case "Получить информацию с датчика влажности":
                            Command = new GetInfoForArduino("http://192.168.1.115", "80");
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                            break;

                        case "Настройки автополива по времени и влажности":
                            Command = new SetParamForArduino("http://192.168.1.115", "80");
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                            break;

                        case "Включить ручной полив":
                            Command = new ArduinoTakeWater("http://192.168.1.115", "80");
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                            break;

                        case "Выключить ручной полив":
                            Command = new ArduinoStopWater("http://192.168.1.115", "80");
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");

                            break;

                        default:

                            var replyKeyboard = new ReplyKeyboardMarkup(
                                new List<KeyboardButton[]>()
                                {
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("Получить информацию с датчика влажности")
                                    },
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("Настройки автополива по времени и влажности")
                                    },
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("Включить ручной полив"),
                                        new KeyboardButton("Выключить ручной полив"),
                                    }
                                })
                            {
                                ResizeKeyboard = true,
                            };
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Стартовое меню:", replyMarkup: replyKeyboard); 

                            break;
                    }

                    break;
                    
                default:
                    break;

            }
            return;
        }

        internal void LogBot(Message msg, string addMsg)
        {
            if (addMsg == "")
            {
                Console.WriteLine($"{DateTime.Now}: Пришло сообщение от {msg?.From.Username}:{msg?.From.Id}. Text = {msg?.Text}");
            }else
            {

            }
        }
    }
}
