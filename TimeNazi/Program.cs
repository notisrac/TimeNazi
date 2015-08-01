using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        static void Main(string[] args)
        {
            logger.Debug("App start");
            if (null != args && args.Length > 0)
            {
                int iDelay = 0;
                if (!int.TryParse(args[0], out iDelay))
                {
                    logger.Error("Could not parse first argument \"{0}\"", args[0]);
                }
                if (iDelay > 0)
                {
                    logger.Debug("Delaying startup by {0}sec", iDelay);
                    Thread.Sleep(iDelay * 1000);
                }
            }
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
