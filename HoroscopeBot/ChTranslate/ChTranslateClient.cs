using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HoroscopeBot.ChTranslate;

namespace HoroscopeBot.ChTranslate
{
	public class ChTranslateClient
	{
		public HttpClient client;

		public async Task<TranslateModel> CheapTranslate(string fromlang, string text, string to)
		{
			var client = new HttpClient();
			RequestModel jsonReques = new RequestModel
			{
				fromLang = fromlang,
				text = text,
				to = to,
			};
			var json = JsonConvert.SerializeObject(jsonReques);
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"https://kursova-telegram-horoscope-api.herokuapp.com/ChTranslate?fromLang={fromlang}&text={text}&to={to}"),

                Content = new StringContent(json)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TranslateModel>(result);
		}
	}
}
