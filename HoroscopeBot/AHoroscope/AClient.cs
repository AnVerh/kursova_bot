using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HoroscopeBot.ChTranslate;



namespace HoroscopeBot.AHoroscope
{
    class AClient
    {
        public HttpClient client;
        
        public AClient()
        {
            client = new HttpClient();
        }
        public async Task<AModel> GetAHoro(string sign, string period)
        {
            var client = new HttpClient();
            if (period == "місяць")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://kursova-telegram-horoscope-api.herokuapp.com/MonthlyW?sign={sign}"),
                    //Content - Type: application / json
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AModel>(result);
            }
            else if (period == "тиждень")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:44300/WeeklyW?sign={sign}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AModel>(result);
            }
            else if (period == "сьогодні"||period=="вчора"||period=="завтра")
            {
                ChTranslateClient chTranslateClient = new ChTranslateClient();
                string engperiod = chTranslateClient.CheapTranslate("uk", period, "en").Result.translatedText;
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:44300/DailyW?sign={sign}&day={engperiod}"),
                };
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AModel>(result);
            }
            string error = "something has gone wrong, please check if you have written everything correct;";
            return JsonConvert.DeserializeObject<AModel>(error);


        }
    }
}
