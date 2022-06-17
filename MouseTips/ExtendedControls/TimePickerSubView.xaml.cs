using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using MouseTips.Helper;

namespace MouseTips.ExtendedControls
{
    /// <summary>
    /// TimePickerSubView.xaml の相互作用ロジック
    /// </summary>
    public partial class TimePickerSubView : Window
    {

        public string Ampm { get; set; }

        private string _preHour;
        private string _preMinute;
        private string _preAmpm;

        public TimePickerSubView()
        {
            InitializeComponent();

            var hourList = new ObservableCollection<ScrollText>();
            for (int n = 0; n < 12; n++)
            {
                var ht = (n + 1).ToString("0");
                var hScrollT = new ScrollText(ht);
                hourList.Add(hScrollT);
            }
            this.hourScroll.ScrollList = hourList;

            var minuteList = new ObservableCollection<ScrollText>();
            for (int n = 0; n < 60; n++)
            {
                var mt = n.ToString("00");
                var mScrollT = new ScrollText(mt);
                minuteList.Add(mScrollT);
            }
            this.minuteScroll.ScrollList = minuteList;

            var ampmList = new ObservableCollection<ScrollText>();
            ampmList.Add(new ScrollText("\n"));
            ampmList.Add(new ScrollText(""));
            ampmList.Add(new ScrollText("AM"));
            ampmList.Add(new ScrollText("PM"));
            this.ampmScroll.ScrollList = ampmList;

        }


        private void OnOK(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            SetPreData(_preHour, _preMinute, _preAmpm);
        }

        public SolidColorBrush MouseEnterColor { get; set; }

        public void SetPreData(string preHour, string preMinute, string preAmpm)
        {
            _preHour = preHour;
            _preMinute = preMinute;
            _preAmpm = preAmpm;

            while (preHour != hourScroll.ScrollList[5].Text)
            {
                hourScroll.ScrollList.Move(hourScroll.ScrollList.Count - 1, 0);
            }

            while (preMinute != minuteScroll.ScrollList[5].Text)
            {
                minuteScroll.ScrollList.Move(minuteScroll.ScrollList.Count - 1, 0);
            }

            while (preAmpm != ampmScroll.ScrollList[2].Text)
            {
                ampmScroll.ScrollList.Move(ampmScroll.ScrollList.Count - 1, 0);
            }

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.hourScroll.BlackItem = 5;
            this.minuteScroll.BlackItem = 5;
            this.ampmScroll.BlackItem = 2;

            Rect rect;
            rect.X = this.Left;
            rect.Y = this.Top;
            rect.Width = this.Width;
            rect.Height = this.Height;

            Rect correctRect = WindowPosControl.CorrectionWindow(rect);

            this.Left = correctRect.X;
            this.Top = correctRect.Y;
            this.Width = correctRect.Width;
            this.Height = correctRect.Height;
        }
    }

}
