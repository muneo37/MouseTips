namespace MouseTips
{
    using System.Windows;
    using MouseTips.Views;
    using MouseTips.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            var w = new MainView();
            var vm = new MainViewModel();
            w.DataContext = vm;
            w.Show();

            base.OnStartup(e);
        }

    }
}
