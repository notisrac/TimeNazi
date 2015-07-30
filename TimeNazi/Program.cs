using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeNazi
{
    static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            logger.Debug("App start");
            //if (Environment.OSVersion.Version.Major >= 6)
            //{
            //    SetProcessDPIAware();
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            logger.Debug("Starting Context");
            Application.Run(new TimeNaziApplicationContext());
        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private static extern bool SetProcessDPIAware();
    }
}
