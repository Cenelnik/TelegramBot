using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.ApiArduino.Interfaces;
using TelegramBot.ApiArduino.Classes;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.ApiArduino.Models;

namespace TelegramBot
{
    /// <summary>
    /// Класс, который обрабатывает изменения в чате бота
    /// </summary>
    internal  class UpdateHandler : IUpdateHandler
    {
        private  delegate void Logger(Message msg, string st);
        private IArduinoExecutable Command;
        private string responsToClient = "";
        int _idCurentArduino = 0;
        List<ArduinoModel> CollectionArduins = new List<ArduinoModel>();
        ArduinoModel CurentArduino = new ArduinoModel();


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
                    await Task.Run(() => WetControlOfGroundWorker(update, botClient));
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


        /// <summary>
        /// Класс который отображает доступные методы Арудуино типа WetControlOfGround
        /// </summary>
        /// <param name="update"></param>
        /// <param name="botClient"></param>
        private async void WetControlOfGroundWorker(Update update, ITelegramBotClient botClient)
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
                        case "Получить информацию с датчика влажности" when CurentArduino.Name != "":
                            Command = new GetInfoForArduino(CurentArduino);
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                            break;

                        case "Настройки автополива по времени и влажности" when CurentArduino.Name != "":
                            Command = new SetParamForArduino(CurentArduino);
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                            break;

                        case "Включить ручной полив" when CurentArduino.Name != "":
                            Command = new ArduinoTakeWater(CurentArduino);
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                            break;

                        case "Выключить ручной полив" when CurentArduino.Name != "":
                            Command = new ArduinoStopWater(CurentArduino);
                            responsToClient = await Command.HttpExecAsync(httpClientFactory.CreateClient(), new List<string> { "firstParamCommand", "secondCommand" });
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");

                            break;

                        case "← Предыдущее устройство" when CurentArduino.Name != "":
                            _idCurentArduino = _idCurentArduino == 0 ? (CollectionArduins.Count() - 1) : _idCurentArduino - 1;
                            CurentArduino = CollectionArduins[_idCurentArduino];
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Вами выбрано устройство: {CurentArduino.Name}");
                            break;

                        case "Слудующее устройство →" when CurentArduino.Name != "": 
                            _idCurentArduino = (_idCurentArduino + 1) < CollectionArduins.Count() ? (_idCurentArduino + 1) : 0;
                            CurentArduino = CollectionArduins[_idCurentArduino];
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Вами выбрано устройство: {CurentArduino.Name}");
                            break;

                        default:
                            CollectionArduins = ArduinoModel.GetCollection(@"C:\hlam\VisualStudio\Project1\TelegramBot\TelegramBot\ApiArduino\Conf\ConfigArduino.json");
                            if (CurentArduino.Name == "" && CollectionArduins.Count > 0)
                            {
                                CurentArduino = CollectionArduins.First();
                                _idCurentArduino = 0;
                            }
                            else
                            {
                                if (CollectionArduins.Count > 0) throw new Exception("Пустой конфиг ./Conf/ConfigArduino.json");
                            }
                            var DefaultReplyKeyboard = new ReplyKeyboardMarkup(
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
                                    },
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("← Предыдущее устройство"),
                                        new KeyboardButton("Слудующее устройство →"),
                                    }
                                })
                            {
                                ResizeKeyboard = true,
                            };
                            await botClient.SendTextMessageAsync(message.Chat.Id, $"Меню устройства: {CurentArduino.Name}", replyMarkup: DefaultReplyKeyboard); ; 

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
