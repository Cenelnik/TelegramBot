using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{
    internal static class ErrorHandler
    {
        internal static async Task Exec(TelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            //return Task.CompletedTask;
        }
    }
}
