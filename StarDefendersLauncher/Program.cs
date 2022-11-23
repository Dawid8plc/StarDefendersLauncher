using CefSharp;
using CefSharp.WinForms;
using StarDefendersLauncher.Managers;
using StarDefendersLauncher.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarDefendersLauncher
{
    internal static class Program
    {
        public static string BasePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string Version = "1.0.0";

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if ANYCPU
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif

            Cef.EnableHighDPISupport();

            SettingsManager.Initialize();
            ServersManager.Initialize();

            //GameForm form = new GameForm();
            ProfilePicker form = new ProfilePicker();
            form.Show();
            Application.Run();
        }
    }
}
