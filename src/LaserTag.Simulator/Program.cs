namespace LaserTag.Simulator;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Add exception handling to catch unhandled errors
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += Application_ThreadException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Enable visual styles and compatible text rendering
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        // MessageBox.Show("Application starting...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Diagnostic

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }

    static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
        MessageBox.Show($"Application Thread Error: {e.Exception.Message}\n\n{e.Exception.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"Application Domain Error: {((Exception)e.ExceptionObject).Message}\n\n{((Exception)e.ExceptionObject).StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
