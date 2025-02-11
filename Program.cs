using System;
using System.Windows;

namespace GunboundGameGuard
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                var application = new Application();
                application.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
                application.DispatcherUnhandledException += Application_DispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                application.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro fatal ao iniciar a aplicação: {ex.Message}\n\nDetalhes:\n{ex.StackTrace}", 
                                "Erro Fatal", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
        }

        private static void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Ocorreu um erro não tratado: {e.Exception.Message}\n\nDetalhes:\n{e.Exception.StackTrace}", 
                            "Erro", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
            e.Handled = true;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro fatal: {ex.Message}\n\nDetalhes:\n{ex.StackTrace}", 
                                "Erro Fatal", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Ocorreu um erro fatal desconhecido.", 
                                "Erro Fatal", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
        }
    }
}