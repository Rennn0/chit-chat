using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using client.src.forms;

namespace client
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new AuthorizationForm());
        }
    }
}
