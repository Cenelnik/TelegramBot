using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ApiArduino.Interfaces;
using TelegramBot.ApiArduino.Models;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс реализующий подачу воды для растений через Ардуино.
    /// Реализует IArduinoExecutable.HttpExecAsync.
    /// </summary>
    internal class ArduinoTakeWater : IArduinoExecutable
    {
        private ArduinoModel _arduino;

        public ArduinoTakeWater(ArduinoModel arduino)
        {
            _arduino = arduino;
        }
        private async Task<string> TakeWater(int time, CancellationToken t)
        {
            
            using var client = new HttpClient();
            var result = await client.GetAsync($"{_arduino.Host}/?{_arduino.Comands.Where(c => c.Name == "GetWater").First().ValueComand}", t);
            return result.StatusCode.ToString();
        }


        public async Task<string> HttpExecAsync(HttpClient client, IEnumerable<string> param)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            return $"Начали подачу воды. Статус выполненой задачи {await TakeWater(10, token)}";
        }
    }
}
