using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NektarPodgorine.Web.Models;

namespace NektarPodgorine.Web.Services
{
    public class WeatherApiService
    {
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        private static readonly HttpClient Http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(8)
        };

        private readonly string apiKey;

        public WeatherApiService()
        {
            apiKey = ConfigurationManager.AppSettings["WeatherApiKey"];
        }

        public bool Konfigurisan
        {
            get { return !string.IsNullOrWhiteSpace(apiKey); }
        }

        public async Task<VremeInfo> TrenutnoVreme(double sirina, double duzina)
        {
            if (!Konfigurisan)
            {
                return null;
            }

            var url = string.Format(CultureInfo.InvariantCulture,
                "{0}?lat={1}&lon={2}&units=metric&lang=sr&appid={3}",
                BaseUrl, sirina, duzina, apiKey);

            try
            {
                var odgovor = await Http.GetAsync(url);
                if (!odgovor.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = JObject.Parse(await odgovor.Content.ReadAsStringAsync());

                return new VremeInfo
                {
                    Temperatura = (double)json["main"]["temp"],
                    Vlaznost = (int)json["main"]["humidity"],
                    BrzinaVetra = (double)json["wind"]["speed"],
                    Opis = (string)json["weather"][0]["description"],
                    Ikonica = (string)json["weather"][0]["icon"]
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
