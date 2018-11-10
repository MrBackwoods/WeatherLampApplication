using System;
using Newtonsoft.Json.Linq;
using System.Net;

namespace WeatherLampApplication
{
    public static class WeatherHandler
    {
        // First part of API call to get weather data
        private const string _apiCall = "http://api.openweathermap.org/data/2.5/weather?q=Tampere&units=metric&APPID=";

        // Second part of API call (the key)
        private const string _apiKey = "key";

        // Function for fetching weather info from the service
        public static JObject GetWeatherInformation()
        {
            var client = new WebClient();
            var response = client.DownloadString(_apiCall + _apiKey);
            return JObject.Parse(response);
        }

        // Function for getting sunrise datetime
        public static DateTime GetSunriseTime(JObject weather)
        {
            return UnixTimeStampToDateTime((double)weather["sys"]["sunrise"]);
        }

        // Function for getting sundown datetime
        public static DateTime GetSundownTime(JObject weather)
        {
            return UnixTimeStampToDateTime((double)weather["sys"]["sunset"]);
        }

        // Function for getting temperature float from the JObject
        public static float GetTemp(JObject weather)
        {
            JToken temp = weather["main"]["temp"];
            return (float)temp;
        }

        // Function for getting rain situation boolean from the JObject
        public static bool GetRainInformation(JObject weather)
        {
            string weatherCondition = weather["weather"][0]["main"].ToString();

            if (weatherCondition.ToLower() == "rain" || weatherCondition.ToLower() == "shower rain" || weatherCondition.ToLower() == "thunderstorm" || weatherCondition.ToLower() == "snow")
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        
        // Function for converting timestamp double to datetime object
        private static DateTime UnixTimeStampToDateTime(double timeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
