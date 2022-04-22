using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            for (int n = 0; n < 24; n++)
            {
                var h = n.ToString("00");
                var hourT = new hourText(h);
                HourList.Add(hourT);
            }
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

            HourList.Move(HourList.Count - 1, 0);

        }

        public readonly static DependencyProperty HourListProperty = DependencyProperty.Register("HourList", typeof(ObservableCollection<hourText>), typeof(TimePicker2), new FrameworkPropertyMetadata(default(ObservableCollection<hourText>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<hourText> HourList
        {
            get { return (ObservableCollection<hourText>)GetValue(HourListProperty); }
            set { SetValue(HourListProperty, value); }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                var fadeIn = FindResource("storyboardSlideDown") as Storyboard;
                fadeIn.Begin();
            }
            else
            {

            }

            base.OnMouseWheel(e);
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
