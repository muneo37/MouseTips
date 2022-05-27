namespace MouseTips
{
    using System.Windows;
    using MouseTips.Views;
    using MouseTips.ViewModels;
    using System;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static bool _showSetting = false;
        private SettingView _settingView = new SettingView();
        private SettingViewModel _settingViewModel = new SettingViewModel();

        protected override void OnStartup(StartupEventArgs e)
        {
            var w = new MainView();
            var vm = new MainViewModel();
            w.DataContext = vm;
            w.Show();

            base.OnStartup(e);

            var icon = GetResourceStream(new Uri("icon.ico", UriKind.Relative)).Stream;
            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Icon = new System.Drawing.Icon(icon),
                Text = "MouseTips",
            };
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(NotifyIcon_Click);
            _settingView.DataContext = _settingViewModel;
        }

        private void NotifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (!_showSetting)
                {
                    _settingView.Show();
                    _showSetting = true;
                }
                else
                {
                    _settingView.Visibility = Visibility.Hidden;
                    _showSetting = false;
                }
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Shutdown();
        }

    }
}
