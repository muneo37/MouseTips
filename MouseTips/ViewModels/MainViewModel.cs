using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Threading;

namespace MouseTips.ViewModels
{
    class MainViewModel : NotificationObject
    {
        #region フィールド
        private Queue<double> _queue = new Queue<double>();
        private List<Screen> _screen = new List<Screen>();
        private Point _prePoint;
        private bool _onDisplay;
        private double _screenBottom;
        private bool _fadeIn = false;
        private bool _drop = false;
        private bool _fadeOut = false;
        private string _text;
        private double _windowTop;
        private double _windowLeft;

        #endregion

        #region プロパティ

        public double ScreenBottom
        {
            get => this._screenBottom;
            set => SetProperty(ref this._screenBottom, value);
        }

        public bool FadeIn
        {
            get => this._fadeIn;
            set => SetProperty(ref this._fadeIn, value);
        }

        public bool Drop
        {
            get => this._drop;
            set => SetProperty(ref this._drop, value);
        }

        public bool FadeOut
        {
            get => this._fadeOut;
            set => SetProperty(ref this._fadeOut, value);
        }

        public string Text
        {
            get => this._text;
            set => SetProperty(ref this._text, value);
        }

        public double WindowTop
        {
            get => this._windowTop;
            set => SetProperty(ref this._windowTop, value);
        }

        public double WindowLeft
        {
            get => this._windowLeft;
            set => SetProperty(ref this._windowLeft, value);
        }

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
                        ScreenBottom = s.Bounds.Height;
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
        public MainViewModel()
        {
            foreach (Screen s in Screen.AllScreens)
            {
                _screen.Add(s);
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Start();
        }

        /// <summary>
        /// FadeInアニメーション完了イベント
        /// </summary>
        public void FadeInCompEvent()
        {
            Drop = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DropCompEvent()
        {
            FadeOut = true;
        }

        /// <summary>
        /// FadeOutアニメーション完了イベント
        /// </summary>
        public void FadeOutCompEvent()
        {
            _onDisplay = false;
            FadeIn = false;
            Drop = false;
            FadeOut = false;
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
                    WindowTop = point.Y;
                    WindowLeft = point.X;
                    var dayOfWeek = DateTime.Now.DayOfWeek.ToString();
                    Text = DateTime.Now.ToString("MM/dd\r\n") + dayOfWeek + DateTime.Now.ToString("\r\nHH:mm:ss");
                    FadeIn = true;
                }
            }
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
