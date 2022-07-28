using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopeBot.CHoroscope
{
    class CClient
    {
        public HttpClient client;
        public CClient()
        {
            client = new HttpClient();
        }
       
        public async Task<CModel> GetCHoro(string sign, string period)
        {
            if(period == "місяць")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://kursova-telegram-horoscope-api.herokuapp.com/MonthlyCh?sign={sign}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CModel>(result);
            }
            else if (period == "тиждень")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://kursova-telegram-horoscope-api.herokuapp.com/WeeklyCh?sign={sign}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CModel>(result);
            }
            else if(period=="сьогодні"||period =="завтра"|| period == "вчора")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://kursova-telegram-horoscope-api.herokuapp.com/DailyCh?sign={sign}&day={period}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CModel>(result);
            }
            var error = "something has gone wrong, please check if you have written everything correct";
            return JsonConvert.DeserializeObject<CModel>(error);
        }
    }
}
