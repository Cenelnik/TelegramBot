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
    /// </summary>
    internal class ArduinoTakeWater : IArduinoExecutable
    {
        internal async Task<string> TakeWater(int time, CancellationToken t)
        {
            
            using var client = new HttpClient();

            var result = await client.GetAsync("http://webcode.me");
            return result.StatusCode.ToString();
        }

        public async Task<string> ExecAsync(IArduinoConnectable arduino, IEnumerable<string> param)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            arduino.WifiConnect();
            //await TakeWater(10, token);
            return $"Начали подачу воды на {param.First()}. Статус выполненой задачи {await TakeWater(10, token)}";
            
        }
    }
}
