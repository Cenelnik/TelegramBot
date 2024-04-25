using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ApiArduino.Interfaces;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс реализующий подачу воды для растений через Ардуино.
    /// Реализует IArduinoExecutable.HttpExecAsync.
    /// </summary>
    internal class ArduinoTakeWater : IArduinoExecutable
    {
        private string _host;
        private string _port;

        public ArduinoTakeWater(string host = @"http://192.168.1.15", string port = "80")
        {
            _host = host;
            _port = port;
        }
        private async Task<string> TakeWater(int time, CancellationToken t)
        {
            
            using var client = new HttpClient();
            var result = await client.GetAsync($"{_host}/?RelayPumpIn1=1", t);
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
