using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Text;

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
        private static ObservableCollection<Tips> _tipsItems = new ObservableCollection<Tips>();
        private int _archiveIndex;
        private bool _appMenuChecked;
        private string _playContent = "stop";
        private bool _playChecked = true;

        #endregion

        #region プロパティ

        public static bool Play { get; private set; } = true;

        public static ObservableCollection<Tips> TipsItems
        {
            get => _tipsItems;
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

        public bool AppMenuChecked
        {
            get => this._appMenuChecked;
            set => SetProperty(ref this._appMenuChecked, value);
        }

        public string PlayContent
        {
            get => this._playContent;
            set => SetProperty(ref this._playContent, value);
        }

        public bool PlayChecked
        {
            get => this._playChecked;
            set
            {
                if (value)
                {
                    this.PlayContent = "stop";
                }
                else
                {
                    this.PlayContent = "play";
                }
                SetProperty(ref _playChecked, value);
                Play = value;
            }
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
                        TipsItems[(int)p].Archive = true;
                        TipsItems[(int)p].MenuChecked = false;
                        this._archiveIndex = (int)p;
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
                        var tips = new Tips(textbox.Text);
                        tips.OnSaveTisp += SaveTips;
                        TipsItems.Add(tips);

                        SaveTips();

                        textbox.Text = "";

                    }));
            }
        }

        private DelegateCommand _removeTipsCommand;
        public DelegateCommand RemoveTipsCommand
        {
            get
            {
                return this._removeTipsCommand ?? (this._removeTipsCommand = new DelegateCommand(
                    p =>
                    {
                        var tips = p as Tips;
                        TipsItems.Remove(tips);
                        SaveTips();
                    }));
            }
        }


        private DelegateCommand _exitCommand;
        public DelegateCommand ExitCommand
        {
            get
            {
                return this._exitCommand ?? (this._exitCommand = new DelegateCommand(
                    p =>
                    {
                        System.Windows.Application.Current.Shutdown();
                    }));
            }
        }

        private DelegateCommand _resetCommand;
        public DelegateCommand ResetCommand
        {
            get
            {
                return this._resetCommand ?? (this._resetCommand = new DelegateCommand(
                    p =>
                    {
                        foreach (Tips tips in TipsItems)
                        {
                            tips.Archive = false;
                            tips.IsVisible = Visibility.Visible;
                            tips.Reset = true;
                        }
                    }));
            }
        }

        private DelegateCommand _saveTipsCommand;
        public DelegateCommand SaveTipsCommand
        {
            get
            {
                return this._saveTipsCommand ?? (this._saveTipsCommand = new DelegateCommand(
                    p =>
                    {
                        SaveTips();
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
            string exe = Directory.GetCurrentDirectory() + "\\MouseTips.exe";

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

            LoadTips();
        }

        private void LoadTips()
        {
            string dir = Directory.GetCurrentDirectory();

            TipsItems.Clear();

            var csvFile = dir + "\\conf.txt";
            if (File.Exists(csvFile))
            {
                var sr = new StreamReader(csvFile);
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    string[] arr = line.Split(',');

                    Tips tip = new Tips(arr[0] + arr[1]);
                    tip.OnSaveTisp += SaveTips;
                    tip.StartTime = arr[2];
                    tip.StopTime = arr[3];

                    TipsItems.Add(tip);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(csvFile + " is not found.");
            }
        }

        public void SaveTips()
        {
            var dir = Directory.GetCurrentDirectory();
            var csvFile = dir + "\\conf.txt";

            var sw = new StreamWriter(csvFile, false, Encoding.UTF8);

            foreach (Tips tips in TipsItems)
            {
                var tipsStr = tips.BigText + "," + tips.SubText + "," + tips.StartTime + "," + tips.StopTime;
                sw.WriteLine(tipsStr);
            }

            sw.Close();
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

            if ((this._archiveIndex + 1) == TipsItems.Count)
            {
                SlideUpCompEvent();
            }
            else
            {
                for (int index = this._archiveIndex + 1; index < TipsItems.Count; index++)
                {
                    TipsItems[index].SlideUp = true;
                }
            }

            foreach (Tips tips in TipsItems)
            {
                tips.SlideUp = false;
            }
        }

        public void SlideUpCompEvent()
        {
            foreach (Tips tips in TipsItems)
            {
                if (tips.Archive == true)
                {
                    tips.IsVisible = Visibility.Collapsed;
                }
            }
        }

        public void TipsResetCompEvent()
        {
            string dir = Directory.GetCurrentDirectory();

            LoadTips();

        }
        #endregion

    }

    /// <summary>
    /// Tipsクラス
    /// </summary>
    class Tips : NotificationObject
    {
        private bool _archive;
        private bool _slideUp;
        private bool _menuChecked;
        private bool _reset = false;
        private Visibility _isVisible = Visibility.Visible;
        private string _startTime;
        private string _stopTime;

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
        public bool MenuChecked
        {
            get => this._menuChecked;
            set => SetProperty(ref this._menuChecked, value);
        }

        public string StartTime
        {
            get => this._startTime;
            set => SetProperty(ref this._startTime, value);
        }
        public string StopTime
        {
            get => this._stopTime;
            set => SetProperty(ref this._stopTime, value);
        }
        public bool Reset
        {
            get => this._reset;
            set => SetProperty(ref this._reset, value);
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

        #region デリゲート
        public delegate void OnSaveTispDelegate();
        public OnSaveTispDelegate OnSaveTisp;
        #endregion

    }
}
