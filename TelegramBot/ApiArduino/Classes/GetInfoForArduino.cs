﻿using System;
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
        private string _host;
        private string _port;

        public GetInfoForArduino(string host = "http://192.168.1.15", string port = "80")
        {
            _host = host;
            _port = port;
        }
        /// <summary>
        /// На ардуине поднят http сервис, который отдает данные с датчика влажности.
        /// Значение датчика от 0 до 1024
        /// </summary>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        internal async Task<string> GetInfo(HttpClient client, CancellationToken t)
        {
            using HttpResponseMessage result =  await client.GetAsync(_host, t);
            string responseBody = await result.Content.ReadAsStringAsync(t);
            return $"Влажность: { GetValue(responseBody, "Влажность:")}\nВключать полив при влажности менее чем: { GetValue(responseBody, "Включать полив при влажности менее чем:")}\nВремя: { GetValue(responseBody, "Время:")}\n";
        }

        public async Task<string> HttpExecAsync(HttpClient client, IEnumerable<string> param)
        {
            try
            {
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken token = cancelTokenSource.Token;
                return await Task.Run(() => GetInfo(client, token));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message} \n {ex.StackTrace}" );
                return $"Удаленный Ардуино {_host} не доступен.";
            }
        }

        private string GetValue(string httpResponce, string httpParamName)
        {
            string value = "";
            value = httpResponce.Substring(httpResponce.IndexOf(httpParamName) + httpParamName.Length);
            value = value.Substring(0, value.IndexOf("</p>"));
            return value.Replace('\n', ' ');
        }
    }
}
