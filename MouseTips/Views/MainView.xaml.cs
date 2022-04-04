using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MouseTips.Views
{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        #region フィールド
        private Queue<double> _queue = new Queue<double>();
        private Point _prePoint;
        private bool _onDisplay;
        private List<Screen> _screen = new List<Screen>();
        private double _screenBottom;
        #endregion


        #region ヘルパー
        /// <summary>
        /// マウス座標取得
        /// </summary>
        /// <returns>マウス座標</returns>
        private static Point GetMousePosition()
        {
            Win32Point win32Point = new Win32Point();
            GetCursorPos(ref win32Point);
            return new Point(win32Point.X, win32Point.Y);
        }

        /// <summary>
        /// マウスが高速移動したか
        /// </summary>
        /// <param name="point">マウス座標値</param>
        /// <returns>True:速い！False:遅い</returns>
        private bool MouseFirst(Point point)
        {
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
            if (val > 40)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 画面上部にマウスが移動した
        /// </summary>
        /// <param name="point">マウス座標位置</param>
        /// <returns></returns>
        private bool OnScreenTop(Point point)
        {
            foreach (Screen s in _screen)
            {
                if (s.Bounds.X < point.X && point.X < (s.Bounds.X + s.Bounds.Width))
                {
                    if (point.Y < s.Bounds.Y + 5)
                    {
                        _screenBottom = s.Bounds.Height;
                        return true;
                    }
                }
            }
            return false;

        }


        #endregion


        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainView()
        {
            InitializeComponent();

            foreach (Screen s in Screen.AllScreens)
            {
                _screen.Add(s);
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Start();
        }
        #endregion



        #region イベント
        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!_onDisplay)
            {
                //座標値取得
                Point point = GetMousePosition();

                if (MouseFirst(point))
                {//マウスが高速移動した

                    return;
                }

                if (OnScreenTop(point))
                {//画面の上にマウスが来た
                    _onDisplay = true;
                    this.Top = point.Y;
                    this.Left = point.X;
                    var dayOfWeek = DateTime.Now.DayOfWeek.ToString();
                    this.timeText.Text = DateTime.Now.ToString("MM/dd\r\n") + dayOfWeek + DateTime.Now.ToString("\r\nHH:mm:ss");
                    var fadeIn = FindResource("storyboardFadeIn") as Storyboard;
                    fadeIn.Begin();
                }
            }
        }

        /// <summary>
        /// フェードインアニメーション完了イベント
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void FadeIn_Complated(object sender, EventArgs e)
        {
            var fadeOut = FindResource("storyboardFadeOut") as Storyboard;
            var frame = new DoubleAnimation();
            frame.From = 0;
            frame.To = _screenBottom;
            frame.Duration = new Duration(TimeSpan.FromSeconds(3));
            frame.EasingFunction = new BounceEase();
            Storyboard.SetTargetName(frame, "timeText");
            Storyboard.SetTargetProperty(frame, new PropertyPath(Canvas.TopProperty));
            fadeOut.Children.Add(frame);
            fadeOut.Begin();
        }

        /// <summary>
        /// フェードアウトアニメーション完了イベント
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータ</param>
        private void FadeOut_Complated(object sender, EventArgs e)
        {
            _onDisplay = false;
        }
        #endregion


        #region Win32API
        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pt);


        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public int X;
            public int Y;
        }
        #endregion

    }
}
