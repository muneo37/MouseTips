using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        #endregion

        #region コマンド
        private DelegateCommand _loadedCommand;

        public DelegateCommand LoadedCommand
        {
            get
            {
                return this._loadedCommand ?? (this._loadedCommand = new DelegateCommand(
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
            foreach (Screen s in Screen.AllScreens)
            {
                _screen.Add(s);
            }

            WindowBottom = _screen[0].Bounds.Height;
            WindowLeft = _screen[0].Bounds.Width - 450;
            ViewTop = WindowBottom - 350;
        }
        #endregion
    }
}
