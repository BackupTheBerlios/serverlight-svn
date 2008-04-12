using System;
using System.Windows.Forms;

namespace ServerLight
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://blogs.developpeur.org/max/");
        }
    }
}