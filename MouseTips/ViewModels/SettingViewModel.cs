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
        private double _windowTop;
        private double _windowLeft;
        #endregion

        #region プロパティ

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

            WindowTop = _screen[0].Bounds.Height - 350;
            WindowLeft = _screen[0].Bounds.Width - 450;
        }
        #endregion
    }
}
