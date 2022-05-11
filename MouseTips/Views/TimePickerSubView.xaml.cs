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
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
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
            }
            else
            {
                var SlideUp = FindResource("storyboardSlideUp") as Storyboard;
                SlideUp.Begin();
                this.pm.Foreground = Brushes.Black;
                this.am.Foreground = this.Foreground;
            }
        }
    }
}
