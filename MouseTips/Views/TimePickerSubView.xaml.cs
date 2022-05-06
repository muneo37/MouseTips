using System.Collections.ObjectModel;
using System.Windows;

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

            var HourList = new ObservableCollection<ScrollText>();

            for (int n = 0; n < 24; n++)
            {
                var t = n.ToString("00");
                var ScrollT = new ScrollText(t);
                HourList.Add(ScrollT);
            }

            this.hourScroll.ScrollList = HourList;
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
