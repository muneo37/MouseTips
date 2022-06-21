using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows;

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
        private ObservableCollection<Tips> _TipsItems = new ObservableCollection<Tips>();
        private int _archiveIndex;
        #endregion

        #region プロパティ

        public ObservableCollection<Tips> TipsItems
        {
            get => this._TipsItems;
            set => SetProperty(ref this._TipsItems, value);
        }

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

        private DelegateCommand _mouseUpCommand;
        public DelegateCommand MouseUpCommand
        {
            get
            {
                return this._mouseUpCommand ?? (this._mouseUpCommand = new DelegateCommand(
                    p =>
                    {
                        Tips t = p as Tips;
                        foreach (var tips in TipsItems.Select((v, i) => new { v, i }))
                        {
                            if (tips.v.Text == t.Text)
                            {
                                tips.v.Archive = true;
                                this._archiveIndex = tips.i;
                            }
                        }
                    }));
            }
        }

        private DelegateCommand _inputTextCommand;
        public DelegateCommand InputTextCommand
        {
            get
            {
                return this._inputTextCommand ?? (this._inputTextCommand = new DelegateCommand(
                    p =>
                    {
                        var textbox = p as System.Windows.Controls.TextBox;
                        TipsItems.Add(new Tips(textbox.Text));
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

            //Todo
            TipsItems.Add(new Tips("最初"));
            TipsItems.Add(new Tips("2番目"));
            TipsItems.Add(new Tips("晴れ"));
            //TipsItems.Add(new Tips("テスト用のコメント"));
            //TipsItems.Add(new Tips("テストです。"));
        }

        /// <summary>
        /// スライド表示完了
        /// </summary>
        public void ShowSlideCompEvent()
        {
            SlideUp = false;
            WindowBottom = _preWindowBottom;
        }

        public void SlideLeftCompEvent()
        {
            for (int index = this._archiveIndex + 1; index < TipsItems.Count; index++)
            {
                TipsItems[index].SlideUp = true;
            }

            foreach (Tips tips in TipsItems)
            {
                tips.SlideUp = false;
            }
        }

        public void SlideUpCompEvent()
        {
            TipsItems[this._archiveIndex].IsVisible = Visibility.Collapsed;
        }
        #endregion
    }

    class Tips : NotificationObject
    {
        private bool _archive;
        private bool _slideUp;
        private Visibility _isVisible = Visibility.Visible;

        public string BigText { get; set; }
        public string SubText { get; set; }
        public string Text { get; set; }
        public bool Archive
        {
            get => this._archive;
            set => SetProperty(ref this._archive, value);
        }
        public bool SlideUp
        {
            get => this._slideUp;
            set => SetProperty(ref this._slideUp, value);
        }
        public Visibility IsVisible
        {
            get => this._isVisible;
            set => SetProperty(ref this._isVisible, value);
        }

        private DelegateCommand _tipsMenuCommand;
        public DelegateCommand TipsMenuCommand
        {
            get
            {
                return this._tipsMenuCommand ?? (this._tipsMenuCommand = new DelegateCommand(
                    p =>
                    {
                        var ret = App.Instance.ShowTipsSettingView();
                    }));

            }
        }

        public Tips(string text)
        {
            if (text.Length <= 3)
            {
                BigText = text;
            }
            else
            {
                BigText = text.Substring(0, 3);
                SubText = text.Substring(3, (text.Length - 3));
            }
            Text = text;
        }

    }
}
