using System.Windows;

namespace reliability
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow main = new MainWindow(new MainWindowVM());
            main.Show();
        }
        
    }
}
