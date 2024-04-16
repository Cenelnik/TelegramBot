using TelegramBot.ApiArduino.Interfaces;

namespace TelegramBot.ApiArduino.Classes
{
    internal class ConnectToArduino : IArduinoConnectable
    {
        public enum TypeConnect 
        {
            WIFI, 
            BLUETOOTH,
            COM
        }

        private TypeConnect _typeConnect = TypeConnect.WIFI;

        private List<string> _connectedParam = new List<string>();
        public ConnectToArduino(IEnumerable<string> param, TypeConnect type)
        {
            _typeConnect = type;

            foreach(var item in param)
            {
                _connectedParam.Add(item);
            }
        }
        bool IArduinoConnectable.BlueToothConnect()
        {
            Console.WriteLine($"Подключились по BlueToothConnect (Заглушка).");
            return true;
        }

        bool IArduinoConnectable.ComConnect()
        {
            Console.WriteLine($"Подключились по COM-PORT (Заглушка).");
            return true;
        }

        bool IArduinoConnectable.WifiConnect()
        {
            Console.WriteLine($"Подключились по wifi (Заглушка).");
            return true;
        }
    }
}
