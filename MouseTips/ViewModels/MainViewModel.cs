using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Threading;
using MouseTips.ViewModels;

namespace MouseTips.ViewModels
{
    class MainViewModel : NotificationObject
    {
        #region フィールド
        private Queue<double> _queue = new Queue<double>();
        private List<Screen> _screen = new List<Screen>();
        private Point _prePoint;
        private bool _onMouseMoveDisplay;
        private bool _onTopDisplay;
        private double _screenBottom;
        private bool _fadeIn = false;
        private bool _drop = false;
        private bool _fadeOut = false;
        private string _text;
        private double _windowTop;
        private double _windowLeft;
        private int _mouseFirstCount;
        DispatcherTimer _readyTimer = new DispatcherTimer();

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
            if (val > 30)
            {
                foreach (Screen s in _screen)
                {
                    if (s.Bounds.X < point.X && point.X < (s.Bounds.X + s.Bounds.Width))
                    {
                        ScreenBottom = s.Bounds.Height;
                    }
                }
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

        private DateTime ConvertTime(string time)
        {
            var now = DateTime.Now;
            if (time == "" || time == null)
            {
                return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            }

            var amPm = time.Substring(0, 2);
            var timeNum = time.Substring(2, time.Length - 2);
            string[] arr = timeNum.Split(":");

            int hour = 0, minute = 0;
            if (amPm == "PM")
            {
                if (arr[0] != "12")
                {
                    if (!int.TryParse(arr[0], out hour))
                    {
                        return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    }
                    hour = hour + 12;
                }
                else
                {
                    if (arr[0] == "12")
                    {
                        hour = 0;
                    }
                    else
                    {
                        if (!int.TryParse(arr[0], out hour))
                        {
                            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                        }
                    }
                }
            }
            else
            {
                if (!int.TryParse(arr[0], out hour))
                {
                    return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                }
            }


            if (!int.TryParse(arr[1], out minute))
            {
                return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            }

            return new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
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
            FadeIn = false;
            Drop = false;
            FadeOut = false;
            _onTopDisplay = false;

            _readyTimer.Tick += ready_Tick;
            _readyTimer.Interval = TimeSpan.FromMinutes(10);
            _readyTimer.Start();
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
            if (SettingViewModel.Play == true)
            {
                //座標値取得
                Point point = GetMousePosition();

                if (MouseFirst(point))
                {//マウスが高速移動した
                    if (!_onMouseMoveDisplay)
                    {
                        if (SettingViewModel.TipsItems.Count == 0)
                        {
                            return;
                        }

                        if (SettingViewModel.TipsItems.Count <= _mouseFirstCount)
                        {
                            _mouseFirstCount = 0;
                        }

                        if (SettingViewModel.TipsItems[_mouseFirstCount].Archive == false)
                        {

                            DateTime now = DateTime.Now;
                            if (SettingViewModel.TipsItems[_mouseFirstCount].StartTime != "")
                            {
                                var startTime = ConvertTime(SettingViewModel.TipsItems[_mouseFirstCount].StartTime);
                                if (now < startTime)
                                {
                                    _mouseFirstCount++;
                                    return;
                                }
                            }
                            if (SettingViewModel.TipsItems[_mouseFirstCount].StopTime != "" && SettingViewModel.TipsItems[_mouseFirstCount].StopTime != null)
                            {
                                var stopTime = ConvertTime(SettingViewModel.TipsItems[_mouseFirstCount].StopTime);
                                if (stopTime < now)
                                {
                                    _mouseFirstCount++;
                                    return;
                                }
                            }
                            Text = SettingViewModel.TipsItems[_mouseFirstCount].Text;
                            _onMouseMoveDisplay = true;
                            WindowTop = point.Y;
                            WindowLeft = point.X;
                            FadeIn = true;
                        }
                        _mouseFirstCount++;
                    }
                    return;
                }

                if (OnScreenTop(point))
                {//画面の上にマウスが来た
                    if (!_onTopDisplay)
                    {
                        _onTopDisplay = true;
                        WindowTop = point.Y;
                        WindowLeft = point.X;
                        var dayOfWeek = DateTime.Now.DayOfWeek.ToString();
                        Text = DateTime.Now.ToString("MM/dd\r\n") + dayOfWeek + DateTime.Now.ToString("\r\nHH:mm:ss");
                        FadeIn = true;
                    }
                }
            }
        }

        private void ready_Tick(object sender, EventArgs e)
        {
            _onMouseMoveDisplay = false;
            _readyTimer.Stop();
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
