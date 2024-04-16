using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.ApiArduino.Interfaces
{
    /// <summary>
    /// Любой объект для работы с Ардуино должен уметь подключаться к ней
    /// Происходить это может по разным интерфейсам: BlueTooth, Wifi, COM-port
    /// </summary>
    public interface IArduinoConnectable
    {
       
        /// <summary>
        /// Метод, который производит подключение по Wifi к плате.
        /// Параметры нужно указывать в конструкторе класса
        /// Возвращает true при успехе
        /// </summary>
        /// <returns></returns>
        public bool WifiConnect();

        /// <summary>
        /// Метод, который производит подключение по BlueTooth к плате.
        /// Параметры нужно указывать в конструкторе класса
        /// Возвращает true при успехе
        /// </summary>
        /// <returns></returns>
        public bool BlueToothConnect();

        /// <summary>
        /// Метод, который производит подключение по COM-Port`у к плате.
        /// Параметры нужно указывать в конструкторе класса.
        /// Возвращает true при успехе
        /// </summary>
        /// <returns></returns>
        public bool ComConnect();
    }
}
