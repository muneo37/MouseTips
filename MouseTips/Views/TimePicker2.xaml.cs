using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MouseTips;

namespace MouseTips.Views
{
    /// <summary>
    /// TimePicker2.xaml の相互作用ロジック
    /// </summary>
    public partial class TimePicker2 : UserControl
    {
        public TimePicker2()
        {
            InitializeComponent();

            HourList = new ObservableCollection<hourText>();
            var hour0 = new hourText("00");
            HourList.Add(hour0);

            var hour1 = new hourText("01");
            HourList.Add(hour1);

            var hour2 = new hourText("02");
            HourList.Add(hour2);

            var hour3 = new hourText("03");
            HourList.Add(hour3);

        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var fadeIn = FindResource("storyboardSlideDown") as Storyboard;
            fadeIn.Begin();
        }

        private void SlideDown_Completed(object sender, EventArgs e)
        {
            var reset = FindResource("storyboardSlideReset") as Storyboard;
            reset.Begin();
            HourList[1] = new hourText("00");
            HourList[2] = new hourText("01");
            HourList[3] = new hourText("02");
        }

        private void SlideReset_Completed(object sender, EventArgs e)
        {


        }

        public DependencyProperty HourListProperty = DependencyProperty.Register("HourList", typeof(ObservableCollection<hourText>), typeof(TimePicker2), new FrameworkPropertyMetadata(default(ObservableCollection<hourText>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<hourText> HourList
        {
            get { return (ObservableCollection<hourText>)GetValue(HourListProperty); }
            set { SetValue(HourListProperty, value); }
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {

        }

        private void Storyboard_Completed_1(object sender, EventArgs e)
        {

        }
    }

    public class hourText
    {
        public hourText(string text)
        {
            Text = text;
        }
        public string Text { get; set; }
    }
}
