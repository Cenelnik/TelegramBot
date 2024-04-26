using TelegramBot.ApiArduino.Interfaces;
using TelegramBot.ApiArduino.Models;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс который отвечает за принудительную остановку подачи воды через Ардуино. 
    /// Реализует IArduinoExecutable.HttpExecAsync.
    /// </summary>
    internal class ArduinoStopWater : IArduinoExecutable
    {

        private ArduinoModel _arduino;

        public ArduinoStopWater(ArduinoModel arduino)
        {
            _arduino = arduino;
        }

        private async Task<string> StopWater(int time, CancellationToken t)
        {

            using var client = new HttpClient();
            var result = await client.GetAsync($"{_arduino.Host}/?{_arduino.Comands.Where(c => c.Name == "StopWater").First().ValueComand}", t);
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
