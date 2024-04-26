using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace TelegramBot
{
    /// <summary>
    /// Класс для обработки команд из бота
    /// </summary>
    class BackServer
    {
        private static string tokenForBot = "7139650096:AAHvu2G2U_HtIPrBcasE5JsE-02C9HN8YBM";
        private static TelegramBotClient ?_botClient;
        private static ReceiverOptions ?_receiverOptions;

        public static async Task Main(string[] args)
        {
            _botClient = new TelegramBotClient(tokenForBot);
            _receiverOptions = new ReceiverOptions 
            {
                AllowedUpdates = new[] 
                {
                    UpdateType.Message, 
                },
                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

            _botClient.StartReceiving(new UpdateHandler(), _receiverOptions, cts.Token); 

            var me = await _botClient.GetMeAsync(); 
            
            Console.WriteLine($"{me.FirstName} запущен!");

            await Task.Delay(-1); 
        }
    }
}
