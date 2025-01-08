using client.forms;
using LLibrary.Guards;

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
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            ApplicationConfiguration.Initialize();

            int affected = LocalSettings.Init();
            Application.Run(new Authenticator());
        }

        private static void TaskScheduler_UnobservedTaskException(
            object? sender,
            UnobservedTaskExceptionEventArgs e
        )
        {
            MessageBox.Show(e.Exception.Message, @"Task Error", MessageBoxButtons.OK);
            e.SetObserved();
        }

        private static void CurrentDomain_UnhandledException(
            object sender,
            UnhandledExceptionEventArgs e
        )
        {
            MessageBox.Show(
                ((Exception)e.ExceptionObject).Message,
                @"Domain Error",
                MessageBoxButtons.OK
            );
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, @"Thread Error", MessageBoxButtons.OK);
        }
    }
}
