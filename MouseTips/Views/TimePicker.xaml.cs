using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        }


        private void OnClick(object sender, RoutedEventArgs e)
        {

            if (hourText.Text == "hour") hourText.Text = "1";
            if (minuteText.Text == "minute") minuteText.Text = "00";

            _subView.SetPreData(hourText.Text, minuteText.Text, AmPmText.Text);
            _subView.ShowDialog();

            this.hourText.Text = _subView.hourScroll.ScrollList[5].Text;
            this.minuteText.Text = _subView.minuteScroll.ScrollList[5].Text;
            this.AmPmText.Text = _subView.ampmScroll.ScrollList[2].Text;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var point = this.PointToScreen(new(0.0d, 0.0d));

            _subView.Background = (SolidColorBrush)this.FindResource("DarkBackground");
            _subView.Foreground = (SolidColorBrush)this.FindResource("DarkForeground");
            _subView.MouseEnterColor = (SolidColorBrush)this.FindResource("DarkMouseEnter");
            _subView.Left = point.X;
            _subView.Top = point.Y - 120;
            _subView.Width = this.Width;
            _subView.FontStyle = this.FontStyle;
        }
    }
}
