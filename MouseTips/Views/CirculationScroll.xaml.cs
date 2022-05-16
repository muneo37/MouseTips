﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                if (ScrollList[0].Text != "")
                {
                    OnSlide = true;
                    var slideDown = FindResource("storyboardSlideDown") as Storyboard;
                    slideDown.Begin();
                }
            }
            else
            {
                if (ScrollList[ScrollList.Count - 1].Text != "")
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
