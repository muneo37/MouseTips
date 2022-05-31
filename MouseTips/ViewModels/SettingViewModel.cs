using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MouseTips.ViewModels
{
    class SettingViewModel : NotificationObject
    {

        #region フィールド
        private List<Screen> _screen = new List<Screen>();
        private double _windowBottom;
        private double _viewTop;
        private double _windowLeft;
        private bool _slideUp;
        private double _preWindowBottom;
        private BitmapSource _iconImage;
        #endregion

        #region プロパティ

        public double WindowBottom
        {
            get => this._windowBottom;
            set => SetProperty(ref this._windowBottom, value);
        }
        public double ViewTop
        {
            get => this._viewTop;
            set => SetProperty(ref this._viewTop, value);
        }
        public double WindowLeft
        {
            get => this._windowLeft;
            set => SetProperty(ref this._windowLeft, value);
        }
        public bool SlideUp
        {
            get => this._slideUp;
            set => SetProperty(ref this._slideUp, value);
        }

        public BitmapSource IconImage
        {
            get => this._iconImage;
            set => SetProperty(ref this._iconImage, value);
        }

        #endregion

        #region コマンド
        private DelegateCommand _visibleChangedCommand;

        public DelegateCommand VisibleChangedCommand
        {
            get
            {
                return this._visibleChangedCommand ?? (this._visibleChangedCommand = new DelegateCommand(
                    p =>
                    {
                        SlideUp = true;
                    }));
            }
        }
        #endregion

        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingViewModel()
        {
            //exeアイコンを表示
            string dir = Directory.GetCurrentDirectory();
            string exe = dir + "\\MouseTips.exe";

            var path = @exe;
            var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
            IconImage = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            foreach (Screen s in Screen.AllScreens)
            {
                _screen.Add(s);
            }

            _preWindowBottom = WindowBottom = _screen[0].Bounds.Height;
            WindowLeft = _screen[0].Bounds.Width - 450;
            ViewTop = WindowBottom - 350;
        }

        /// <summary>
        /// スライド表示完了
        /// </summary>
        public void ShowSlideCompEvent()
        {
            SlideUp = false;
            WindowBottom = _preWindowBottom;
        }
        #endregion
    }
}
