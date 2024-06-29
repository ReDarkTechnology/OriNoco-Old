using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OriNoco
{
    public partial class EditorForm : Form
    {
        #region Variables
        public VS2015DarkTheme Theme { get; set; } = new VS2015DarkTheme();
        public EditorView Editor { get; set; }
        public GameView Game { get; set; }
        public ChartView Chart { get; set; }
        public ColorReference ColorReference { get; set; } = new ColorReference();
        public event Action OnUpdate;

        bool wasFocus;
        #endregion

        #region Initialization
        public EditorForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            ControlUtil.AddDraggableHandle(titleBar, Handle);
            ControlUtil.AddDraggableHandle(menuStrip, Handle);
            mainDockPanel.Theme = Theme;
            Theme.ApplyTo(menuStrip);
            Application.AddMessageFilter(new MouseMessageFilter());

            ColorReference.BorderColor = Color.FromArgb(0, 122, 204);
            this.AddBorder(Point.Empty, ColorReference);
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            ShowEditorView();
            ShowGameView();
            ShowChartView();
            Editor.Show();
        }
        #endregion

        #region Engine
        private void gameUpdate_Tick(object sender, EventArgs e)
        {
            Console.WriteLine($"ContainsFocus: " + ContainsFocus.ToString());
            if (wasFocus != ContainsFocus)
            {
                wasFocus = ContainsFocus;
                UpdateWindowBorder(ContainsFocus && WindowState != FormWindowState.Maximized);
            }

            Time.Update();
            OnUpdate?.Invoke();
        }
        #endregion

        #region Window
        public void ShowEditorView()
        {
            if (Editor == null)
                Editor = new EditorView(this);
            Editor.Show(mainDockPanel, DockState.Document);
        }

        public void ShowGameView()
        {
            if (Game == null)
                Game = new GameView();
            Game.Show(mainDockPanel, DockState.Document);
        }

        public void ShowChartView()
        {
            if (Chart == null)
                Chart = new ChartView();
            Chart.Show(mainDockPanel, DockState.DockRight);
        }
        #endregion

        #region Titlebar
        private void UpdateMaxWinIcon() => titleBarMaxWinBtn.Image = WindowState == FormWindowState.Maximized ? OriNocoResources.Windowed : OriNocoResources.Full;
        private void titleBarMaxWinBtn_Click(object sender, EventArgs e) => ToggleWindowState();
        private void titleBarMinBtn_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;
        private void titleBar_DoubleClick(object sender, EventArgs e) => ToggleWindowState();
        private void titleBarCloseBtn_Click(object sender, EventArgs e) => Application.Exit();

        public void ToggleWindowState()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                WindowState = FormWindowState.Maximized;
            }
            UpdateMaxWinIcon();
        }

        private void mainPanel_Resize(object sender, EventArgs e)
        {
            UpdateWindowBorder(WindowState != FormWindowState.Maximized);
            UpdateMaxWinIcon();
        }

        public void UpdateWindowBorder(bool windowed)
        {
            ColorReference.BorderColor = windowed ? Color.FromArgb(0, 122, 204) : Color.FromArgb(45, 45, 48);
            Refresh();
        }

        protected override void WndProc(ref Message m)
        {
            const int wmNcHitTest = 0x84;
            const int htLeft = 10;
            const int htRight = 11;
            const int htTop = 12;
            const int htTopLeft = 13;
            const int htTopRight = 14;
            const int htBottom = 15;
            const int htBottomLeft = 16;
            const int htBottomRight = 17;

            if (m.Msg == wmNcHitTest)
            {
                int x = (int)(m.LParam.ToInt64() & 0xFFFF);
                int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
                Point pt = PointToClient(new Point(x, y));
                Size clientSize = ClientSize;
                // Lower right corner
                if (pt.X >= clientSize.Width - 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomLeft : htBottomRight);
                    return;
                }
                // Lower left corner
                if (pt.X <= 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomRight : htBottomLeft);
                    return;
                }
                // Upper right corner
                if (pt.X <= 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopRight : htTopLeft);
                    return;
                }
                // Left corner
                if (pt.X >= clientSize.Width - 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopLeft : htTopRight);
                    return;
                }
                // Top border
                if (pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htTop);
                    return;
                }
                // Bottom border
                if (pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htBottom);
                    return;
                }
                // Left border
                if (pt.X <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htLeft);
                    return;
                }
                // Right border
                if (pt.X >= clientSize.Width - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htRight);
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public class MouseMessageFilter : IMessageFilter
        {
            private const int WM_MOUSEMOVE = 0x0200;

            public static event MouseEventHandler MouseMove;

            public bool PreFilterMessage(ref Message m)
            {

                if (m.Msg == WM_MOUSEMOVE)
                {
                    if (MouseMove != null)
                    {
                        int x = Control.MousePosition.X, y = Control.MousePosition.Y;

                        MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                    }
                }
                return false;
            }
        }
        #endregion
    }
}
