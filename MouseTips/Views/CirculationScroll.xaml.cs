using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MouseTips.Views
{
    /// <summary>
    /// CirculationScroll.xaml の相互作用ロジック
    /// </summary>
    public partial class CirculationScroll : UserControl
    {
        private bool OnSlide = false;

        public CirculationScroll()
        {
            InitializeComponent();

            ScrollList = new ObservableCollection<ScrollText>();

        }

        public readonly static DependencyProperty ScrollListProperty = DependencyProperty.Register("ScrollList", typeof(ObservableCollection<ScrollText>), typeof(CirculationScroll), new FrameworkPropertyMetadata(default(ObservableCollection<ScrollText>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<ScrollText> ScrollList
        {
            get { return (ObservableCollection<ScrollText>)GetValue(ScrollListProperty); }
            set { SetValue(ScrollListProperty, value); }
        }

        //Todo 依存関係である必要ない？
        public readonly static DependencyProperty BlackItemProperty = DependencyProperty.Register("BlackItem", typeof(int), typeof(CirculationScroll), new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int BlackItem
        {
            get { return (int)GetValue(BlackItemProperty); }
            set
            {
                var trigger = new Trigger();
                trigger.Property = ItemsControl.AlternationIndexProperty;
                trigger.Value = value;
                trigger.Setters.Add(new Setter(Label.ForegroundProperty, new SolidColorBrush(Colors.Black), "label1"));

                var label = new FrameworkElementFactory(typeof(Label));
                label.SetValue(Label.WidthProperty, slideCanvas.ActualWidth);
                label.SetValue(Label.HeightProperty, (double)30);
                label.SetValue(Label.VerticalAlignmentProperty, VerticalAlignment.Center);
                label.SetValue(Label.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                label.SetValue(Label.ForegroundProperty, this.Foreground);
                Binding binding = new Binding("Text");
                label.SetBinding(Label.ContentProperty, binding);
                label.Name = "label1";

                var dataTemplate = new DataTemplate(typeof(ItemsControl));
                dataTemplate.VisualTree = label;
                dataTemplate.Triggers.Add(trigger);
                scrollItems.ItemTemplate = dataTemplate;

                SetValue(BlackItemProperty, value);
            }
        }

        private void SlideDown_Completed(object sender, EventArgs e)
        {
            var reset = FindResource("storyboardSlideReset") as Storyboard;
            reset.Begin();

            ScrollList.Move(ScrollList.Count - 1, 0);
            OnSlide = false;
        }

        private void SlideUp_Completed(object sender, EventArgs e)
        {
            var reset = FindResource("storyboardSlideReset") as Storyboard;
            reset.Begin();

            ScrollList.Move(0, ScrollList.Count - 1);
            OnSlide = false;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (OnSlide == true) return;

            if (e.Delta > 0)
            {
                if (ScrollList[0].Text != "\n")
                {
                    OnSlide = true;
                    var slideDown = FindResource("storyboardSlideDown") as Storyboard;
                    slideDown.Begin();
                }
            }
            else
            {
                if (ScrollList[ScrollList.Count - 1].Text != "\n")
                {
                    OnSlide = true;
                    var SlideUp = FindResource("storyboardSlideUp") as Storyboard;
                    SlideUp.Begin();
                }
            }

            base.OnMouseWheel(e);
        }
    }

    public class ScrollText
    {
        public ScrollText(string text)
        {
            Text = text;
        }
        public string Text { get; set; }
    }

}
