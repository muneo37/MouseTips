using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MouseTips.ExtendedControls
{
    /// <summary>
    /// TimePicker.xaml の相互作用ロジック
    /// </summary>
    public partial class TimePicker : UserControl
    {
        private TimePickerSubView _subView = new TimePickerSubView();

        public readonly static DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(TimePicker), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTimePropertyChanged)));

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set
            {
                SetValue(TimeProperty, value);
                SetTime(value);
            }
        }

        public ICommand MyCommand
        {
            get { return (ICommand)GetValue(MyCommandProperty); }
            set { SetValue(MyCommandProperty, value); }
        }
        public static readonly DependencyProperty MyCommandProperty =
            DependencyProperty.Register(
                "MyCommand",                    // プロパティ名
                typeof(ICommand),               // プロパティの型
                typeof(TimePicker),      // プロパティを所有する型＝このクラスの名前
                new PropertyMetadata(null));    // 初期値

        private static void OnTimePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TimePicker timePicker = sender as TimePicker;

            timePicker.Time = e.NewValue as string;
        }

        private void SetTime(string time)
        {
            if (time == "")
            {
                return;
            }
            this.AmPmText.Text = time.Substring(0, 2);
            var timeNum = time.Substring(2, time.Length - 2);
            string[] arr = timeNum.Split(":");
            this.hourText.Text = arr[0];
            this.minuteText.Text = arr[1];
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

            var point = this.PointToScreen(new(0.0d, 0.0d));
            _subView.Left = point.X;
            _subView.Top = point.Y - 120;
            _subView.SetPreData(hourText.Text, minuteText.Text, AmPmText.Text);
            _subView.ShowDialog();

            this.hourText.Text = _subView.hourScroll.ScrollList[5].Text;
            this.minuteText.Text = _subView.minuteScroll.ScrollList[5].Text;
            this.AmPmText.Text = _subView.ampmScroll.ScrollList[2].Text;

            Time = AmPmText.Text + hourText.Text + ":" + minuteText.Text;
            MyCommand.Execute(this);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            _subView.Background = (SolidColorBrush)this.FindResource("TimeDarkBackground");
            _subView.Foreground = (SolidColorBrush)this.FindResource("TimeDarkForeground");
            _subView.MouseEnterColor = (SolidColorBrush)this.FindResource("TimeDarkMouseEnter");
            _subView.Width = this.Width;
            _subView.FontStyle = this.FontStyle;
        }
    }
}
