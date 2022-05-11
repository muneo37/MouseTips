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

        private TimePickerSubView _subView = new TimePickerSubView();

        public readonly static DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimePicker), new FrameworkPropertyMetadata(default(TimeSpan), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }


        public TimePicker()
        {
            InitializeComponent();

            this.hourText.Text = "hour";
            this.minuteText.Text = "minute";
            this.AmPmText.Text = "AM";

            _subView.CloseHandler = OnSubViewClose;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var point = this.PointToScreen(new(0.0d, 0.0d));

            _subView.Left = point.X;
            _subView.Top = point.Y - 120;

            _subView.Background = (SolidColorBrush)this.FindResource("DarkBackground");
            _subView.Foreground = (SolidColorBrush)this.FindResource("DarkForeground");
            _subView.Width = this.Width;
            _subView.FontStyle = this.FontStyle;
            _subView.MouseEnterColor = (SolidColorBrush)this.FindResource("DarkMouseEnter");

            _subView.ShowDialog();
        }

        static void OnSubViewClose(EventArgs args)
        {
            int test = 1;
        }
    }
}
