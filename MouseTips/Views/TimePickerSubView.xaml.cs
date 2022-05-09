using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

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

            var ampmList = new ObservableCollection<ScrollText>();
            ampmList.Add(new ScrollText("AM"));
            ampmList.Add(new ScrollText("PM"));
            this.ampmScroll.ScrollList = ampmList;
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

    }
}
