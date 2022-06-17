using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace MouseTips.Helper
{
    class WindowPosControl
    {

        /// <summary>
        /// モニターのスクリーン内に収まるRectを返す。デュアルモニタ未対応
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static Rect CorrectionWindow(Rect rect)
        {

            List<Screen> screen = new List<Screen>();

            foreach (Screen s in Screen.AllScreens)
            {
                screen.Add(s);
            }

            if (screen[0].Bounds.Width < (rect.X + rect.Width))
            {
                rect.X = screen[0].Bounds.Width - rect.Width;
            }

            if (screen[0].Bounds.Height < (rect.Y + rect.Height))
            {
                rect.Y = screen[0].Bounds.Height - rect.Height;
            }

            return rect;
        }
    }
}
