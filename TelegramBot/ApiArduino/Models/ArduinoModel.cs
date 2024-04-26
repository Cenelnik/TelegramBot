
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace TelegramBot.ApiArduino.Models
{
    /// <summary>
    /// Класс который моделирует настройки из конфига по ардуино
    /// </summary>
    internal class ArduinoModel
    {
        private string name = "";
        [JsonPropertyName("name")]
        public string Name 
        { 
            get { return name; }
            set { name = value; } 
        }
        private string host = "";
        [JsonPropertyName("host")]
        public string Host 
        {
            get { return host; }
            set { host = value; }
        }
        
        private string port = "";
        [JsonPropertyName("port")]
        public string Port 
        {
            get { return port; }
            set { port = value; }
        }
        
        private string type = "";
        [JsonPropertyName("type")]
        public string Type 
        {
            get { return type; }
            set { type = value; }
        }
        private List<ComandModel> comands = new List<ComandModel> { };
        [JsonPropertyName("comands")]
        public List<ComandModel> Comands 
        {
            get { return comands;}
            set { comands = value;}
        }



        public static List<ArduinoModel> GetCollection (string pathToConfig)
        {
            List<ArduinoModel> Collection = new List<ArduinoModel>();
            try
            {
                using (var StreamFromConfig = File.OpenRead(pathToConfig))
                {
                    byte[] Buffer = new byte[StreamFromConfig.Length];
                    StreamFromConfig.Read(Buffer, 0, Buffer.Length);
                    string ConfigValue = Encoding.Default.GetString(Buffer);
                    JObject jObject = JObject.Parse(ConfigValue);
                    JToken list = jObject["ArduinoDevice"];
                    foreach(var item in list)
                    {
                        ArduinoModel a = item.ToObject<ArduinoModel>();
                        Collection.Add(a);
                    }
                }
                return Collection != null ? Collection:throw new Exception($"Конфиг пустой -> надо проверить конфиг.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG = {ex.Message}.\nTRACE = {ex.StackTrace}.");
                return Collection;
            }
            
        }

    }
}
