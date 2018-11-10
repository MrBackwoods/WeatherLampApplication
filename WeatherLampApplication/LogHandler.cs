using System;
using System.Windows.Forms;

namespace WeatherLampApplication
{
    class LogHandler
    {
        // List box
        public static ListBox logBox = new ListBox();

        // Function for adding new log row to list box
        public static void WriteToLog(string message)
        {
            if (logBox.InvokeRequired)
            {
                logBox.Invoke(new MethodInvoker(delegate{logBox.Items.Insert(0, (DateTime.Now.ToString() + ": " + message));}));
            }
            else
            {
                logBox.Items.Insert(0, (DateTime.Now.ToString() + ": " + message));
            }
        }
    }
}
