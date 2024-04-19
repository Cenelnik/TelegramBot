using TelegramBot.ApiArduino.Interfaces;

namespace TelegramBot.ApiArduino.Classes
{
    /// <summary>
    /// Класс который отвечает за принудительную остановку подачи воды. 
    /// </summary>
    internal class ArduinoStopWater : IArduinoExecutable
    {

        private string Executer(IEnumerable<string> param)
        {
            return $"Остановка подачи воды через {param.First()}";
        }

        public async Task<string> ExecAsync(IArduinoConnectable arduino, IEnumerable<string> param)
        {
            arduino.WifiConnect();
            return await Task.Run(() => Executer(param));
        }
    }
}
