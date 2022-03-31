﻿namespace MouseTips.Views
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        private Queue<double> _queue = new Queue<double>();
        private Point _prePoint;
        private bool _onDisplay;

        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pt);

        public MainView()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Point point = GetMousePosition();
            double x = this._prePoint.X - point.X;
            double y = this._prePoint.Y - point.Y;
            double nextLength = Math.Sqrt(x * x + y * y);
            this._queue.Enqueue(nextLength);
            if (this._queue.Count > 15)
            {
                this._queue.Dequeue();
            }
            this._prePoint = point;
            double total = 0;
            foreach (double length in _queue)
            {
                total += length;
            }

            var val = total / 30;
            if ((val > 40) && (!_onDisplay) ) 
            {
                _onDisplay = true;
                this.Top = point.Y;
                this.Left = point.X;
                var dayOfWeek = DateTime.Now.DayOfWeek.ToString();
                this.timeText.Text = DateTime.Now.ToString("MM/dd\r\n") + dayOfWeek + DateTime.Now.ToString("\r\nHH:mm:ss");
                var fadeIn = FindResource("storyboardFadeIn") as Storyboard;
                fadeIn.Begin();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public int X;
            public int Y;
        }

        public static Point GetMousePosition()
        {
            Win32Point win32Point = new Win32Point();
            GetCursorPos(ref win32Point);
            return new Point(win32Point.X, win32Point.Y);
        }

        private void FadeIn_Complated(object sender, EventArgs e)
        {
            var fadeOut = FindResource("storyboardFadeOut") as Storyboard;
            fadeOut.Begin();
        }

        private void FadeOut_Complated(object sender, EventArgs e)
        {
            _onDisplay = false;
        }
    }
}