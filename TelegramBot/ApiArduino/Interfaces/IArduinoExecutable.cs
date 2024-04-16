using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.ApiArduino.Interfaces
{
    /// <summary>
    /// Объект умеет давать команды в ардуино и получать от нее некий ответ. 
    /// </summary>
    public interface IArduinoExecutable
    {
        /// <summary>
        /// Метод реализующий какую-либо команду для Ардуино. 
        /// На вход подаем объект arduino, к которому мы подсоединились и коллекцию строк. 
        /// На выходе ожидаем от Ардуино какой то ответ в виде строки.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Exec(IArduinoConnectable arduino, IEnumerable<string> param);
    }
}
