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
        public string Exec(IArduinoConnectable arduino, IEnumerable<string> param)
        {
            arduino.WifiConnect();
            return $"Начали подачу воды на {param.First()}";
        }
    }
}
