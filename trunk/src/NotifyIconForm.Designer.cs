namespace ServerLight
{
    partial class NotifyIconForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyIconForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNotiFyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.openRootDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripNotiFyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "ServerLight";
            this.notifyIcon1.BalloonTipTitle = "Double-Clic to open Default Browser";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStripNotiFyIcon;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ServerLight";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStripNotiFyIcon
            // 
            this.contextMenuStripNotiFyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3,
            this.openRootDirectoryToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.contextMenuStripNotiFyIcon.Name = "contextMenuStripNotiFyIcon";
            this.contextMenuStripNotiFyIcon.Size = new System.Drawing.Size(185, 104);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(181, 6);
            // 
            // openRootDirectoryToolStripMenuItem
            // 
            this.openRootDirectoryToolStripMenuItem.Image = global::ServerLight.Properties.Resources.folder_go;
            this.openRootDirectoryToolStripMenuItem.Name = "openRootDirectoryToolStripMenuItem";
            this.openRootDirectoryToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openRootDirectoryToolStripMenuItem.Text = "Open Root Directory";
            this.openRootDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openRootDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::ServerLight.Properties.Resources.information;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem1.Text = "About";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::ServerLight.Properties.Resources.stop;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.closeToolStripMenuItem.Text = "&Stop";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // NotifyIconForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(115, 26);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NotifyIconForm";
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.Text = "NotifyIcon";
            this.contextMenuStripNotiFyIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNotiFyIcon;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openRootDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}