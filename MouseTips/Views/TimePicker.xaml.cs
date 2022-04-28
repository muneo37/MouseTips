using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MouseTips.Views
{
    /// <summary>
    /// TimePicker.xaml の相互作用ロジック
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();

            this.hourText.Text = "hour";
            this.minuteText.Text = "minute";
            this.AmPmText.Text = "AM";
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var subView = new TimePickerSubView();
            var point = this.PointToScreen(new(0.0d, 0.0d));

            subView.Left = point.X;
            subView.Top = point.Y;

            subView.Background = (SolidColorBrush)this.FindResource("DarkBackground");
            subView.Foreground = (SolidColorBrush)this.FindResource("DarkForeground");
            subView.Width = this.Width;

            subView.ShowDialog();
        }
    }
}
