using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ApiArduino.Interfaces;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс реализующий получение информации от Ардуино: настройки, данные с датчиков.
    /// </summary>
    internal class GetInfoForArduino : IArduinoExecutable
    {
        public string Exec(IArduinoConnectable arduino, IEnumerable<string> param)
        {
            arduino.WifiConnect();
            return "Передали настройки и данные с датчиков";
        }
    }
}
