using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OriNoco
{
    public partial class EditorView : DockContent
    {
        public EditorForm EditorForm { get; set; }
        public EditorRenderer Renderer { get; set; }

        public EditorView(EditorForm form)
        {
            InitializeComponent();

            Renderer = new EditorRenderer();
            Renderer.Initialize(viewParent);

            EditorForm = form;
            EditorForm.OnUpdate += EditorForm_OnUpdate;
        }

        private void EditorForm_OnUpdate()
        {
            Renderer.Run();
        }
    }
}
