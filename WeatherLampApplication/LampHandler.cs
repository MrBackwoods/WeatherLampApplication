using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using YeelightAPI;
using YeelightAPI.Models.ColorFlow;
using System.Threading;

namespace WeatherLampApplication
{
    static class LampHandler
    {
        // Yeelight lamp device
        private static Device lamp = new Device("10.0.0.4");

        // Yeelight lamp helper variables
        private static int[] lampColor = new int[] { 0, 0, 0 };
        private static int lampBrightness = 0;
        private static bool lampFlicker = false;
        private static int flickerAmount = 25;
        private static int lampFlowSpeed = 2000;
        private static int maxBrightnes = 80;

        // Function that sets new lamp color, flicker and brightness
        public static void UpdateLamp(bool weatherLampEnabled)
        {
            LogHandler.WriteToLog("Updating lamp status");

            if (weatherLampEnabled)
            {
                if (!lamp.IsConnected)
                {
                    ConnectToLamp();
                }

                JObject weather = WeatherHandler.GetWeatherInformation();

                float temperature = WeatherHandler.GetTemp(weather);
                HandleColor(temperature);

                DateTime sunriseTime = WeatherHandler.GetSunriseTime(weather);
                DateTime sundownTime = WeatherHandler.GetSundownTime(weather);
                int newLampBrightness = CalculateBrightness(sunriseTime, sundownTime);
                HandleBrightness(newLampBrightness);

                bool isRaining = WeatherHandler.GetRainInformation(weather);
                HandleFlicker(isRaining);
            }

            else
            {
                ResetLamp();
            }
        }

        // Functionality for setting lamp color
        public static void HandleColor(float temperature)
        {
            LogHandler.WriteToLog("Temperature outside: " + temperature + " °C");

            int[] newLampColor = CalculateColor(temperature);

            if (!newLampColor.SequenceEqual(lampColor))
            {
                lampColor = newLampColor;
                lamp.SetRGBColor(lampColor[0], lampColor[1], lampColor[2], lampFlowSpeed);
                LogHandler.WriteToLog("New lamp color set: " + String.Join(",", lampColor));
            }

            else
            {
                LogHandler.WriteToLog("No changes made to lamp color");
            }
        }

        // Functionality for setting lamp brightness
        public static void HandleBrightness(int newBrightness)
        {
            if (newBrightness == maxBrightnes - 50)
            {
                LogHandler.WriteToLog("Sun is up");
            }

            else if (newBrightness == maxBrightnes)
            {
                LogHandler.WriteToLog("Sun is down");
            }

            else
            {
                LogHandler.WriteToLog("Sun is rising or setting");
            }

            if (lampBrightness != newBrightness)
            {
                lampBrightness = newBrightness;
                lamp.SetBrightness(lampBrightness, lampFlowSpeed);
                LogHandler.WriteToLog("New lamp brightness set: " + lampBrightness.ToString() + " %");
            }

            else
            {
                LogHandler.WriteToLog("No changes made to lamp brightness");
            }
        }

        // Functionality for setting lamp flicker
        public static void HandleFlicker(bool isRaining)
        {
            if (isRaining)
            {
                LogHandler.WriteToLog("It's raining");
            }

            else
            {
                LogHandler.WriteToLog("It's not raining");
            }

            if (isRaining && !lampFlicker)
            {
                SetFlicker(true);
                LogHandler.WriteToLog("Lamp flicker enabled because it's raining");
            }

            else if (!isRaining && lampFlicker)
            {
                SetFlicker(false);
                LogHandler.WriteToLog("Lamp flicker disabled because it's not raining");
            }

            else
            {
                LogHandler.WriteToLog("No changes made to lamp flicker");
            }
        }

        // Function for resetting lamp
        public static void ResetLamp()
        {
            lampColor = new int[] { 0, 0, 0 };
            lampFlicker = false;
            lampBrightness = 100;
            lamp.SetRGBColor(lampColor[0], lampColor[1], lampColor[2], lampFlowSpeed);
            lamp.SetBrightness(lampBrightness, lampFlowSpeed);
        }

        // Function to connect to the lamp and turning it on
        private static void ConnectToLamp()
        {
            lamp.Connect();

            while (!lamp.IsConnected)
            {
                LogHandler.WriteToLog("Connecting to lamp at " + lamp.Hostname);
                Thread.Sleep(1000);
            }

            LogHandler.WriteToLog("Connected to lamp at " + lamp.Hostname);
            lamp.TurnOn();
            LogHandler.WriteToLog("Lamp has been turned on");
        }

        // Function for setting lamp flicker
        private static void SetFlicker(bool enable)
        {
            if (enable)
            {
                ColorFlow flow = new ColorFlow(0, ColorFlowEndAction.Restore);
                flow.Add(new ColorFlowRGBExpression(lampColor[0], lampColor[1], lampColor[1], lampBrightness, 2000));
                flow.Add(new ColorFlowRGBExpression(lampColor[0], lampColor[1], lampColor[2], lampBrightness - flickerAmount, 1000));
                lamp.StartColorFlow(flow);
                lampFlicker = true;
            }

            else
            {
                lampFlicker = false;
                lamp.StopColorFlow();
            }
        }

        // Function calculating brightness for the lamp
        private static int CalculateBrightness(DateTime sunriseTime, DateTime sundownTime)
        {
            int brightness = maxBrightnes - 50 ;

            DateTime now = DateTime.Now;

            if (DateTime.Compare(now, sunriseTime) < 0)
            {
                double minutesToSunrise = (sunriseTime - now).TotalMinutes;

                if (minutesToSunrise <= 10)
                {
                    brightness = maxBrightnes - 40;
                }

                else if (minutesToSunrise <= 20)
                {
                    brightness = maxBrightnes - 30;
                }

                else if (minutesToSunrise <= 30)
                {
                    brightness = maxBrightnes - 20;
                }

                else if (minutesToSunrise <= 40)
                {
                    brightness = 10;
                }

                else
                {
                    brightness = maxBrightnes;
                }
            }

            else if (DateTime.Compare(now, sundownTime) > 0)
            {
                double minutesFromSundown = (now - sundownTime).TotalMinutes;

                if (minutesFromSundown <= 10)
                {
                    brightness = maxBrightnes - 40;
                }

                else if (minutesFromSundown <= 20)
                {
                    brightness = maxBrightnes - 30;
                }

                else if (minutesFromSundown <= 30)
                {
                    brightness = maxBrightnes - 20;
                }

                else if (minutesFromSundown <= 40)
                {
                    brightness = maxBrightnes -10;
                }

                else
                {
                    brightness = maxBrightnes;
                }
            }

            return brightness;
        }

        // Function for calculating color from the temperature - favours towards orange
        private static int[] CalculateColor(float temp)
        {
            if (temp >= 0)
            {
                int colorReductionG = 255 - (int)temp * 6;

                if (colorReductionG < 0)
                {
                    colorReductionG = 0;
                }

                int colorReductionB = 255 - (int)temp * 10;

                if (colorReductionB < 0)
                {
                    colorReductionB = 0;
                }

                int[] color = new int[] { 255, colorReductionG, colorReductionB };
                return color;
            }

            else
            {
                int colorReduction = 255 - (int)temp * -6;

                if (colorReduction < 0)
                {
                    colorReduction = 0;
                }

                int[] color = new int[] { colorReduction, colorReduction, 255 };
                return color;
            }
        }
    }
}
