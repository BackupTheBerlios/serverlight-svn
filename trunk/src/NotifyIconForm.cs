using System;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;
using ServerLight;

namespace ServerLight
{
    internal partial class NotifyIconForm : Form , IMenuService
    {
        private static NotifyIconForm s_NotifyIconForm;

        private NotifyIconForm()
        {
            InitializeComponent();
            Closing += delegate
                          {
                              notifyIcon1.Visible = false;
                              notifyIcon1.Dispose();
                          };
        }

        internal static NotifyIconForm Instance
        {
            get
            {
                if (s_NotifyIconForm == null)
                {
                    s_NotifyIconForm = new NotifyIconForm();
                }
                return s_NotifyIconForm;
            }
        }

        public NotifyIcon NotifyIcon
        {
            get { return notifyIcon1; }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openRootDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerLighConsoletHost.ServerLightInstance.OpenRootDirectory();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ServerLighConsoletHost.ServerLightInstance.LaunchDefaultWebBrowser();

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new InfoForm().ShowDialog();
        }

        #region IMenuService Members

        public void AddToolStripMenuItem(ToolStripMenuItem t)
        {
            this.contextMenuStripNotiFyIcon.Items.Insert(0,t);
        }

        #endregion
    }

    public interface IMenuService
    {
        void AddToolStripMenuItem(ToolStripMenuItem t);
    }
}