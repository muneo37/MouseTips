using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MouseTips.Views
{
    /// <summary>
    /// TimePicker2.xaml の相互作用ロジック
    /// </summary>
    public partial class TimePicker2 : UserControl
    {
        public TimePicker2()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var fadeIn = FindResource("storyboardFadeIn") as Storyboard;
            fadeIn.Begin();
        }
    }
}
