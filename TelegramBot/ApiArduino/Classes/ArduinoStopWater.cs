using TelegramBot.ApiArduino.Interfaces;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс который отвечает за принудительную остановку подачи воды. 
    /// </summary>
    internal class ArduinoStopWater : IArduinoExecutable
    {
        public string Exec(IArduinoConnectable arduino, IEnumerable<string> param)
        {
            arduino.WifiConnect();
            return $"Остановка подачи воды через {param.First()}";
        }
    }
}
