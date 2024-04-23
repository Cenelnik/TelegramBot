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

        private string Executer(IEnumerable<string> param)
        {
            return $"Остановка подачи воды через {param.First()}";
        }

        public async Task<string> HttpExecAsync(HttpClient client, IEnumerable<string> param)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            return await Task.Run(() => Executer(param));
        }
    }
}
