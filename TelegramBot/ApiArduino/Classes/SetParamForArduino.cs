using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TelegramBot.ApiArduino.Interfaces;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс реализующий установку параметров для ардуино
    /// Реализует IArduinoExecutable.HttpExecAsync.
    /// </summary>
    internal class SetParamForArduino : IArduinoExecutable
    {
        private string _host = "195.168.1.15";
        private string _port = "80";

        public SetParamForArduino(string host , string port)
        {
            _host = host;
            _port = port;
        }

        private List<string> _param = new List<string>();
        private string Executer(IEnumerable<string> param)
        {
            foreach (string curParam in param)
            {
                _param.Add(curParam);
            }
            return  $"Установили след. настройки. Полив по времени: {_param[0]} Полив по проценту влажности: {_param[1]}";
        }
        public async Task<string> HttpExecAsync(HttpClient client, IEnumerable<string> param)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            return await Task.Run(() => Executer(param));  
        }
    }
}
