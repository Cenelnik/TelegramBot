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
    /// Предполагается, что можно установить таймер подачи воды или подачу воды по проценту влажности почвы.
    /// </summary>
    internal class SetParamForArduino : IArduinoExecutable
    {
        private List<string> _param = new List<string>();
        private string Executer(IEnumerable<string> param)
        {
            foreach (string curParam in param)
            {
                _param.Add(curParam);
            }
            return  $"Установили след. настройки. Полив по времени: {_param[0]} Полив по проценту влажности: {_param[1]}";
        }
        public async Task<string> ExecAsync(IArduinoConnectable arduino, IEnumerable<string> param)
        {
            
            arduino.WifiConnect();
            return await Task.Run(() => Executer(param));  
        }
    }
}
