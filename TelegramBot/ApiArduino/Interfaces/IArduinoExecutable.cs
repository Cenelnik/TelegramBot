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
        /// Подключаемся к Ардуино по Http и 
        /// Исполняем что то (возможно нужны будут параметры)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public  Task<string> HttpExecAsync(HttpClient client, IEnumerable<string> param);

    }
}
