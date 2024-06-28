using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OriNoco
{
    public partial class EditorForm : Form
    {
        public static ChartView chartScene = new ();
        public EditorForm()
        {
            InitializeComponent();
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            chartScene.Initialize(chartPanel);
        }

        private void gameUpdate_Tick(object sender, EventArgs e)
        {
            Time.Update();
            chartScene.Run();
            var mousePos = chartScene.GetMousePosition();
            mousePosLabel.Text = $"MousePos: {mousePos.x}, {mousePos.y}";
        }
    }
}
