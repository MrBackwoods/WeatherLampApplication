using System;
using System.Drawing;
using System.Windows.Forms;

namespace WeatherLampApplication
{
    public partial class WeatherLampApplicationForm : Form
    {
        // Bool for keeping track if weather lamp functionality is enabled
        public  bool weatherLampEnabled = false;

        // Initializing
        public WeatherLampApplicationForm()
        {
            InitializeComponent();
        }

        // Starting lamp update loop and setting up logging listbox on form load
        private void WeatherLampApplicationForm_Load(object sender, EventArgs ea)
        {
            try
            {
                LogHandler.logBox = LogBox;

                LogHandler.WriteToLog("Application started, weather lamp disabled");

                var timer = new System.Threading.Timer((e) =>
                {
                    if (weatherLampEnabled)
                    {
                        LampHandler.UpdateLamp(weatherLampEnabled);
                    }
                }, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            }

            catch (Exception ex)
            {
                LogHandler.WriteToLog("Exception: " + ex.Message);
            }
        }

        // Enable and disable the lamp app
        private void LampToggleButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!weatherLampEnabled)
                {
                    LogHandler.WriteToLog("Weather lamp enabled");
                    weatherLampEnabled = true;
                    LampToggleButton.Text = "Disable";
                    LampToggleButton.BackColor = Color.PaleVioletRed;
                    LampHandler.UpdateLamp(weatherLampEnabled);
                }

                else
                {
                    LogHandler.WriteToLog("Weather lamp disabeled");
                    weatherLampEnabled = false;
                    LampToggleButton.Text = "Enable";
                    LampToggleButton.BackColor = Color.PaleGreen;
                    LampHandler.UpdateLamp(weatherLampEnabled);
                }
            }

            catch (Exception ex)
            {
                LogHandler.WriteToLog("Exception: " + ex.Message);
            }
        }
    }
}
