
using System.Text.Json.Serialization;

namespace TelegramBot.ApiArduino.Models
{
    /// <summary>
    /// Класс, который моделирует команды из конфига по ардуино. 
    /// </summary>
    internal class ComandModel
    {
        private string name = "";
        [JsonPropertyName("name")]
        public string Name 
        {
            get { return name; }
            set { name = value; }
        }
        private string valuecomand = "";
        [JsonPropertyName("valuecomand")]
        public string ValueComand
        {
            get { return valuecomand; }
            set { valuecomand = value; }
        }
    }
}
