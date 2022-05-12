using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MouseTips.Views
{
    /// <summary>
    /// TimePickerSubView.xaml の相互作用ロジック
    /// </summary>
    public partial class TimePickerSubView : Window
    {

        public string ampm;

        private ObservableCollection<ScrollText> _preHourList = new ObservableCollection<ScrollText>();
        private ObservableCollection<ScrollText> _preMinuteList = new ObservableCollection<ScrollText>();
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

            this.am.Foreground = Brushes.Black;

            SetPreData();
        }


        private void OnOK(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            hourScroll.ScrollList = _preHourList;
            minuteScroll.ScrollList = _preMinuteList;
            ampm = _preAmpm;

            this.Visibility = Visibility.Hidden;
        }

        public SolidColorBrush MouseEnterColor { get; set; }

        private void OnCanvasMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                var slideDown = FindResource("storyboardSlideDown") as Storyboard;
                slideDown.Begin();
                this.am.Foreground = Brushes.Black;
                this.pm.Foreground = this.Foreground;
                ampm = "AM";
            }
            else
            {
                var SlideUp = FindResource("storyboardSlideUp") as Storyboard;
                SlideUp.Begin();
                this.pm.Foreground = Brushes.Black;
                this.am.Foreground = this.Foreground;
                ampm = "PM";
            }
        }

        public void SetPreData()
        {
            _preHourList = hourScroll.ScrollList;
            _preMinuteList = minuteScroll.ScrollList;
            _preAmpm = ampm;
        }
    }
}
