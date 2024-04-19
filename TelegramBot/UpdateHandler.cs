using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.ApiArduino.Interfaces;
using TelegramBot.ApiArduino.Classes;

namespace TelegramBot
{
    internal  class UpdateHandler : IUpdateHandler
    {
        private  delegate void Logger(Message msg, string st);
        private IArduinoConnectable arduino;
        private IArduinoExecutable command;
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
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        //Информация о сообщении
                        var message = update.Message;
                        // From - это от кого пришло сообщение (или любой другой Update)
                        User ?user = message.From;

                        Logger log = LogBot;
                        log(update.Message, "");

                        switch(message.Text)
                        {
                            case "Получить информацию с датчика влажности":
                                arduino = new ConnectToArduino(new List<string> { "firstParamConnect", "secondParamConnect" }, ConnectToArduino.TypeConnect.WIFI);
                                command = new GetInfoForArduino();
                                responsToClient = await command.ExecAsync(arduino, new List<string> { "firstParamCommand", "secondCommand" });
                                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                                break;

                            case "Настройки автополива по времени и влажности":
                                arduino = new ConnectToArduino(new List<string> { "firstParamConnect", "secondParamConnect" }, ConnectToArduino.TypeConnect.WIFI);
                                command = new SetParamForArduino();
                                responsToClient = await command.ExecAsync(arduino, new List<string> { "firstParamCommand", "secondCommand" });
                                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                                break;

                            case "Включить ручной полив":
                                arduino = new ConnectToArduino(new List<string> { "firstParamConnect", "secondParamConnect" }, ConnectToArduino.TypeConnect.WIFI);
                                command = new ArduinoTakeWater();
                                responsToClient = await command.ExecAsync(arduino, new List<string> { "firstParamCommand", "secondCommand" });
                                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"{responsToClient}");
                                break;

                            case "Выключить ручной полив":
                                arduino = new ConnectToArduino(new List<string> { "firstParamConnect", "secondParamConnect" }, ConnectToArduino.TypeConnect.WIFI);
                                command = new ArduinoStopWater();
                                responsToClient = await command.ExecAsync(arduino, new List<string> { "firstParamCommand", "secondCommand" });
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
                                    // автоматическое изменение размера клавиатуры, если не стоит true,
                                    // тогда клавиатура растягивается чуть ли не до луны,
                                    // проверить можете сами
                                    ResizeKeyboard = true,
                                };
                                // опять передаем клавиатуру в параметр replyMarkup
                                await botClient.SendTextMessageAsync(message.Chat.Id, "Стартовое меню:", replyMarkup: replyKeyboard); 

                                break;
                        }
                        

                        return;

                    }
            }
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
