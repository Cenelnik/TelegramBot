using TelegramBot.ApiArduino.Interfaces;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс который отвечает за принудительную остановку подачи воды через Ардуино. 
    /// Реализует IArduinoExecutable.HttpExecAsync.
    /// </summary>
    internal class ArduinoStopWater : IArduinoExecutable
    {

        private string _host = "195.168.1.15";
        private string _port = "80";

        public ArduinoStopWater(string host = @"http://192.168.1.15", string port = "80")
        {
            _host = host;
            _port = port;
        }

        private async Task<string> StopWater(int time, CancellationToken t)
        {

            using var client = new HttpClient();
            var result = await client.GetAsync($"{_host}/?RelayPumpIn1=0", t);
            return result.StatusCode.ToString();
        }

        public async Task<string> HttpExecAsync(HttpClient client, IEnumerable<string> param)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            return $"Остановили подачу воды. Статус выполненой задачи {await StopWater(10, token)}";
        }
    }
}
