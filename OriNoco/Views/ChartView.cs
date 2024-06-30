using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OriNoco
{
    public partial class ChartView : DockContent
    {
        public EditorForm MainForm { get; set; }
        public ChartView(EditorForm form)
        {
            InitializeComponent();

            MainForm = form;
        }
    }
}
