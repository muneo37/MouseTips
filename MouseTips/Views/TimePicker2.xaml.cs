using System;
using System.Collections.Generic;
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

            HourList = new List<hourText>();

            var hour1 = new hourText();
            hour1.Text = "01";
            HourList.Add(hour1);

            var hour2 = new hourText();
            hour2.Text = "02";
            HourList.Add(hour2);

            var hour3 = new hourText();
            hour3.Text = "03";
            HourList.Add(hour3);

            Test = "aaaaa";
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            //var fadeIn = FindResource("storyboardFadeIn") as Storyboard;
            //fadeIn.Begin();

            Test = "bbbbb";
        }

        public static readonly DependencyProperty HourListProperty = DependencyProperty.Register("HourList", typeof(List<hourText>), typeof(TimePicker2));

        public static readonly DependencyProperty TestProperty = DependencyProperty.Register("Test", typeof(string), typeof(TimePicker2));

        public string Test
        {
            get { return (string)GetValue(TestProperty); }
            set { SetValue(TestProperty, value); }
        }

        public List<hourText> HourList
        {
            get { return (List<hourText>)GetValue(HourListProperty); }
            set { SetValue(HourListProperty, value); }
        }
    }

    public class hourText
    {
        public string Text { get; set; }
    }
}
