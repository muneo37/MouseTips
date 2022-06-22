using System.Windows;

namespace MouseTips.Views
{
    /// <summary>
    /// SettingView.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingView : Window
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private void OnDeactibated(object sender, System.EventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.Activate();
            }
        }
    }
}
