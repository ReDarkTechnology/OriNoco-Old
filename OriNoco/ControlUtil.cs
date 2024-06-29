using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OriNoco
{
    static class ControlUtil
    {
        public static DraggableControl AddDraggableHandle(Control interacter, IntPtr handle = default(IntPtr))
        {
            var d = new DraggableControl();
            if (handle == default(IntPtr))
                d.handle = interacter.Handle;
            else
                d.handle = handle;
            interacter.MouseDown += d.Drag_MouseDown;
            interacter.MouseMove += d.Drag_MouseMove;
            interacter.MouseUp += d.Drag_MouseUp;
            return d;
        }

        public static void AddBorder(this Control control)
        {
            var point = new Point(control.DisplayRectangle.X, control.DisplayRectangle.Y);
            AddBorder(control, point);
        }

        public static void AddBorder(this Control control, Point point)
        {
            control.Paint += (a, e) =>
            e.Graphics.DrawRectangle(new Pen(control.ForeColor), point.X, point.Y, control.Size.Width - 1, control.Size.Height - 1);
            control.Resize += (a, e) => control.Refresh();
        }

        public static void AddBorder(this Control control, Point point, ColorReference color)
        {
            control.Paint += (a, e) =>
            e.Graphics.DrawRectangle(new Pen(color.BorderColor), point.X, point.Y, control.Size.Width - 1, control.Size.Height - 1);
            control.Resize += (a, e) => control.Refresh();
        }

        public static void ModernifyInteraction(this Control control, Color mainColor, Color onHoverColor, Color onClickColor)
        {
            control.MouseDown += (a, e) => control.BackColor = onClickColor;
            control.MouseUp += (a, e) => control.BackColor = onHoverColor;
            control.MouseEnter += (a, e) => control.BackColor = onHoverColor;
            control.MouseLeave += (a, e) => control.BackColor = mainColor;
        }
        public static void ModernifyInteraction(this Control control, ColorDictionary dictionary)
        {
            var d = dictionary;
            control.MouseDown += (a, e) => control.BackColor = d.click;
            control.MouseUp += (a, e) => control.BackColor = d.hover;
            control.MouseEnter += (a, e) => control.BackColor = d.hover;
            control.MouseLeave += (a, e) => control.BackColor = d.main;
        }

        public static void ModernifyInteraction(this Control control, Control eventControl, int intensity = 1)
        {
            var d = new ColorDictionary(control, intensity);
            eventControl.MouseDown += (a, e) => control.BackColor = d.click;
            eventControl.MouseUp += (a, e) => control.BackColor = d.hover;
            eventControl.MouseEnter += (a, e) => control.BackColor = d.hover;
            eventControl.MouseLeave += (a, e) => control.BackColor = d.main;
        }

        public static void ModernifyInteraction(this Control control, Control[] controls, int intensity = 1)
        {
            var d = new ColorDictionary(control, intensity);
            control.MouseDown += (a, e) => control.BackColor = d.click;
            control.MouseUp += (a, e) => control.BackColor = d.hover;
            control.MouseEnter += (a, e) => control.BackColor = d.hover;
            control.MouseLeave += (a, e) => {
                if (!MouseIsOverControls(controls)) control.BackColor = d.main;
            };

            foreach (var subCtrl in controls)
            {
                subCtrl.MouseDown += (a, e) => control.BackColor = d.click;
                subCtrl.MouseUp += (a, e) => control.BackColor = d.hover;
                subCtrl.MouseEnter += (a, e) => control.BackColor = d.hover;
                subCtrl.MouseLeave += (a, e) => {
                    if (!MouseIsOverControls(controls)) control.BackColor = d.main;
                };
            }
        }

        public static void ModernifyInteraction(this Control control, List<Control> controls, int intensity = 1)
        {
            var d = new ColorDictionary(control, intensity);
            control.MouseDown += (a, e) => control.BackColor = d.click;
            control.MouseUp += (a, e) => control.BackColor = d.hover;
            control.MouseEnter += (a, e) => control.BackColor = d.hover;
            control.MouseLeave += (a, e) => {
                if (!controls.Exists(val => MouseIsOverControl(val))) control.BackColor = d.main;
            };

            foreach(var subCtrl in controls)
            {
                subCtrl.MouseDown += (a, e) => control.BackColor = d.click;
                subCtrl.MouseUp += (a, e) => control.BackColor = d.hover;
                subCtrl.MouseEnter += (a, e) => control.BackColor = d.hover;
                subCtrl.MouseLeave += (a, e) => {
                    if (!controls.Exists(val => MouseIsOverControl(val))) control.BackColor = d.main;
                };
            }
        }

        private static bool MouseIsOverControls(Control[] ctrls)
        {
            for (int i = 0; i < ctrls.Length; i++)
            {
                if (ctrls[i].ClientRectangle.Contains(ctrls[i].PointToClient(Cursor.Position))) return true;
            }
            return false;
        }

        private static bool MouseIsOverControl(Control ctr) =>
            ctr.ClientRectangle.Contains(ctr.PointToClient(Cursor.Position));

        public static ColorDictionary ModernifyInteraction(this Control control, int intensity = 1)
        {
            var d = new ColorDictionary(control, intensity);
            control.MouseDown += (a, e) => control.BackColor = d.click;
            control.MouseUp += (a, e) => control.BackColor = d.hover;
            control.MouseEnter += (a, e) => control.BackColor = d.hover;
            control.MouseLeave += (a, e) => control.BackColor = d.main;
            return d;
        }

        public static Point Add(this Point lhs, Point rhs)
        {
            return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Color Add(this Color c, int val)
        {
            return Color.FromArgb(c.A, c.R + val, c.G + val, c.B + val);
        }

        #region Insiders
        public class DraggableControl
        {
            public const int WM_NCLBUTTONDOWN = 0xA1;
            public const int HT_CAPTION = 0x2;

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool ReleaseCapture();

            public IntPtr handle;
            private bool queueMove;

            public void Drag_MouseDown(object sender, MouseEventArgs e) => queueMove = true;
            public void Drag_MouseUp(object sender, MouseEventArgs e) => queueMove = false;

            public void Drag_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left && queueMove)
                {
                    ReleaseCapture();
                    SendMessage(handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    queueMove = false;
                }
            }
        }
        #endregion
    }

    public class ColorDictionary
    {
        public Color main = Color.Black;
        public Color hover = Color.DarkGray;
        public Color click = Color.Gray;

        public ColorDictionary() { }
        public ColorDictionary(Control control) : this(control, 1) { }
        public ColorDictionary(Control control, int intensity) => Initialize(control.BackColor);
        public void Initialize(Color color, int intensity = 1)
        {
            main = color;
            hover = color.Add(15 * intensity);
            click = color.Add(45 * intensity);
        }
    }

    public class ColorReference
    {
        public Color BorderColor { get; set; }
    }
}
