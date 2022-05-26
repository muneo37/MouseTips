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

        protected override void OnStartup(StartupEventArgs e)
        {
            var w = new MainView();
            var vm = new MainViewModel();
            w.DataContext = vm;
            w.Show();

            base.OnStartup(e);

            var icon = GetResourceStream(new Uri("icon.ico", UriKind.Relative)).Stream;
            var menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("設定", null, Setting_Click);
            menu.Items.Add("終了", null, Exit_Click);
            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Icon = new System.Drawing.Icon(icon),
                Text = "MouseTips",
                ContextMenuStrip = menu
            };
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(NotifyIcon_Click);
        }

        private void NotifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //var wnd = new MainWindow();
                //wnd.Show();
            }
        }

        private void Setting_Click(object sender, EventArgs e)
        {
            var settingView = new SettingView();
            var settingViewModel = new SettingViewModel();
            settingView.DataContext = settingViewModel;
            settingView.Show();

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Shutdown();
        }

    }
}
