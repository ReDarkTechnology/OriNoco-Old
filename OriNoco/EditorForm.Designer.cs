namespace OriNoco
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.gameUpdate = new System.Windows.Forms.Timer(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.titleBarIcon = new System.Windows.Forms.PictureBox();
            this.mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.titleBar = new System.Windows.Forms.Panel();
            this.titleBarMinBtn = new System.Windows.Forms.Button();
            this.titleBarMaxWinBtn = new System.Windows.Forms.Button();
            this.titleBarCloseBtn = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleBarIcon)).BeginInit();
            this.titleBar.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameUpdate
            // 
            this.gameUpdate.Enabled = true;
            this.gameUpdate.Interval = 1;
            this.gameUpdate.Tick += new System.EventHandler(this.gameUpdate_Tick);
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Controls.Add(this.mainDockPanel);
            this.mainPanel.Controls.Add(this.titleBar);
            this.mainPanel.Location = new System.Drawing.Point(3, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(954, 534);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.Resize += new System.EventHandler(this.mainPanel_Resize);
            // 
            // titleBarIcon
            // 
            this.titleBarIcon.Image = global::OriNoco.OriNocoResources.UpArrow;
            this.titleBarIcon.Location = new System.Drawing.Point(5, 6);
            this.titleBarIcon.Name = "titleBarIcon";
            this.titleBarIcon.Size = new System.Drawing.Size(18, 18);
            this.titleBarIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.titleBarIcon.TabIndex = 2;
            this.titleBarIcon.TabStop = false;
            // 
            // mainDockPanel
            // 
            this.mainDockPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.mainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDockPanel.ForeColor = System.Drawing.Color.White;
            this.mainDockPanel.Location = new System.Drawing.Point(0, 32);
            this.mainDockPanel.Name = "mainDockPanel";
            this.mainDockPanel.Size = new System.Drawing.Size(954, 502);
            this.mainDockPanel.TabIndex = 0;
            // 
            // titleBar
            // 
            this.titleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.titleBar.Controls.Add(this.titleBarMinBtn);
            this.titleBar.Controls.Add(this.titleBarIcon);
            this.titleBar.Controls.Add(this.titleBarMaxWinBtn);
            this.titleBar.Controls.Add(this.titleBarCloseBtn);
            this.titleBar.Controls.Add(this.menuStrip);
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(954, 32);
            this.titleBar.TabIndex = 2;
            this.titleBar.DoubleClick += new System.EventHandler(this.titleBar_DoubleClick);
            // 
            // titleBarMinBtn
            // 
            this.titleBarMinBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleBarMinBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.titleBarMinBtn.FlatAppearance.BorderSize = 0;
            this.titleBarMinBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(75)))));
            this.titleBarMinBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.titleBarMinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBarMinBtn.Image = global::OriNoco.OriNocoResources.Minimize;
            this.titleBarMinBtn.Location = new System.Drawing.Point(856, 0);
            this.titleBarMinBtn.Name = "titleBarMinBtn";
            this.titleBarMinBtn.Size = new System.Drawing.Size(32, 32);
            this.titleBarMinBtn.TabIndex = 4;
            this.titleBarMinBtn.UseVisualStyleBackColor = true;
            this.titleBarMinBtn.Click += new System.EventHandler(this.titleBarMinBtn_Click);
            // 
            // titleBarMaxWinBtn
            // 
            this.titleBarMaxWinBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleBarMaxWinBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.titleBarMaxWinBtn.FlatAppearance.BorderSize = 0;
            this.titleBarMaxWinBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(75)))));
            this.titleBarMaxWinBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.titleBarMaxWinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBarMaxWinBtn.Image = global::OriNoco.OriNocoResources.Full;
            this.titleBarMaxWinBtn.Location = new System.Drawing.Point(889, 0);
            this.titleBarMaxWinBtn.Name = "titleBarMaxWinBtn";
            this.titleBarMaxWinBtn.Size = new System.Drawing.Size(32, 32);
            this.titleBarMaxWinBtn.TabIndex = 3;
            this.titleBarMaxWinBtn.UseVisualStyleBackColor = true;
            this.titleBarMaxWinBtn.Click += new System.EventHandler(this.titleBarMaxWinBtn_Click);
            // 
            // titleBarCloseBtn
            // 
            this.titleBarCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleBarCloseBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.titleBarCloseBtn.FlatAppearance.BorderSize = 0;
            this.titleBarCloseBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(75)))));
            this.titleBarCloseBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.titleBarCloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBarCloseBtn.Image = global::OriNoco.OriNocoResources.Close;
            this.titleBarCloseBtn.Location = new System.Drawing.Point(921, 0);
            this.titleBarCloseBtn.Name = "titleBarCloseBtn";
            this.titleBarCloseBtn.Size = new System.Drawing.Size(32, 32);
            this.titleBarCloseBtn.TabIndex = 2;
            this.titleBarCloseBtn.UseVisualStyleBackColor = true;
            this.titleBarCloseBtn.Click += new System.EventHandler(this.titleBarCloseBtn_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.AutoSize = false;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(25, 3);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(408, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "MainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.opemToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // opemToolStripMenuItem
            // 
            this.opemToolStripMenuItem.Name = "opemToolStripMenuItem";
            this.opemToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.opemToolStripMenuItem.Text = "Open...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // EditorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(960, 540);
            this.Controls.Add(this.mainPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 360);
            this.Name = "EditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OriNoco";
            this.Load += new System.EventHandler(this.EditorForm_Load);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleBarIcon)).EndInit();
            this.titleBar.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer gameUpdate;
        private System.Windows.Forms.Panel mainPanel;
        private WeifenLuo.WinFormsUI.Docking.DockPanel mainDockPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.PictureBox titleBarIcon;
        private System.Windows.Forms.Button titleBarCloseBtn;
        private System.Windows.Forms.Button titleBarMaxWinBtn;
        private System.Windows.Forms.Button titleBarMinBtn;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opemToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

