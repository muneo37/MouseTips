using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MouseTips.Views
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

            this.am.Foreground = Brushes.Black;

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

        private void OnCanvasMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                var slideDown = FindResource("storyboardSlideDown") as Storyboard;
                slideDown.Begin();
                this.am.Foreground = Brushes.Black;
                this.pm.Foreground = this.Foreground;
                Ampm = "AM";
            }
            else
            {
                var SlideUp = FindResource("storyboardSlideUp") as Storyboard;
                SlideUp.Begin();
                this.pm.Foreground = Brushes.Black;
                this.am.Foreground = this.Foreground;
                Ampm = "PM";
            }
        }

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

            Ampm = preAmpm;
            if (preAmpm == "AM")
            {
                Canvas.SetTop(ampmStack, 120);
                Ampm = "AM";
            }
            else
            {
                Canvas.SetTop(ampmStack, 90);
                Ampm = "PM";
            }
        }
    }

}
