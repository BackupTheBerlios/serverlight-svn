// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 1457 $</version>
// </file>

// project created on 2/6/2003 at 11:10 AM
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.Core;

namespace ServerLight
{
    public class ExceptionBox : Form
    {
        private TextBox exceptionTextBox;
        private CheckBox copyErrorCheckBox;
        //private System.Windows.Forms.CheckBox includeSysInfoCheckBox;
        private Label label;
        private PictureBox pictureBox;
        private Exception exceptionThrown;
        private Button reportButton;
        private string message;

        public ExceptionBox(Exception e, string message, bool mustTerminate)
        {
            this.exceptionThrown = e;
            this.message = message;
            InitializeComponent();

            if (mustTerminate)
            {
                closeButton.Visible = false;
                //continueButton.Left -= closeButton.Width - continueButton.Width;
                //continueButton.Width = closeButton.Width;
            }
            exceptionTextBox.Text = GetExceptionDetailMessage();
        }

        private string GetExceptionDetailMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("ServerLight Version    : {0}{1}", Assembly.GetEntryAssembly().GetName().Version, Environment.NewLine);
            stringBuilder.AppendFormat(".NET Version        : {0}{1}", Environment.Version.ToString(), Environment.NewLine);
            stringBuilder.AppendFormat("OS Version          : " + Environment.OSVersion.ToString() + Environment.NewLine);

            string cultureName = null;
            try
            {
                cultureName = CultureInfo.CurrentCulture.Name;
                stringBuilder.AppendFormat("Current culture      : {0} ({1}){2}", CultureInfo.CurrentCulture.EnglishName, cultureName, Environment.NewLine);
            }
            catch
            {}
            try
            {
                if (cultureName == null || !cultureName.StartsWith("fr-FR"))
                {
                    stringBuilder.Append("Current UI language  : fr-FR");
                    stringBuilder.Append(Environment.NewLine);
                }
            }
            catch
            {}
            try
            {
                if (IntPtr.Size != 4)
                {
                    stringBuilder.AppendFormat("Running as ({0}) bit process{1}" + IntPtr.Size*8, Environment.NewLine);
                }
                if (SystemInformation.TerminalServerSession)
                {
                    stringBuilder.Append("Terminal Server Session");
                    stringBuilder.Append(Environment.NewLine);
                }
                if (SystemInformation.BootMode != BootMode.Normal)
                {
                    stringBuilder.AppendFormat("Boot Mode            : {0}{1}", SystemInformation.BootMode, Environment.NewLine);
                }
            }
            catch
            {}
            stringBuilder.AppendFormat("Working Set Memory   : {0}kb{1}", (Environment.WorkingSet/1024), Environment.NewLine);

            if (message != null)
            {
                stringBuilder.Append(message);
                stringBuilder.Append(Environment.NewLine);
            }
            stringBuilder.Append("Exception thrown: ");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(exceptionThrown.ToString());
            return stringBuilder.ToString();
        }

        private void CopyInfoToClipboard()
        {
            if (copyErrorCheckBox.Checked)
            {
                if (Application.OleRequired() == ApartmentState.STA)
                {
                    ClipboardWrapper.SetText(GetExceptionDetailMessage());
                }
                else
                {
                    Thread th = new Thread((ThreadStart) delegate { ClipboardWrapper.SetText(GetExceptionDetailMessage()); });
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                }
            }
        }

        private void reportErrorButtonClick(object sender, EventArgs e)
        {
            CopyInfoToClipboard();

            // clipboard text is too long to be inserted into the mail-url
            string exceptionTitle = "";
            Exception ex = exceptionThrown;
            if (ex != null)
            {
                try
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    exceptionTitle = " (" + ex.GetType().Name + ")";
                }
                catch
                {}
            }
            string url = "mailto:maxence.dislaire@gmail.com?subject=Bug Report"
                         + Uri.EscapeDataString(exceptionTitle)
                         + "&body="
            	+ Uri.EscapeDataString(@"Write a description on how to reproduce the error:" + Environment.NewLine + "Paste the exception text:" + Environment.NewLine);
            StartUrl(url);
            
        }

        private static void StartUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                /*ILoggingService loggingService = ServiceLocator.Instance.GetService<ILoggingService>();
                loggingService.Warn("Cannot start " + url, e);*/
            }
        }

        private void continueButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to quit ServerLight?", "ServerLight", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionBox));
            this.closeButton = new System.Windows.Forms.Button();
            this.label = new System.Windows.Forms.Label();
            this.copyErrorCheckBox = new System.Windows.Forms.CheckBox();
            this.exceptionTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.reportButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(224, 390);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(187, 23);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Exit ServerLight";
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(12, 101);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(405, 32);
            this.label.TabIndex = 6;
            this.label.Text = "An unhandled exception has occurred in ServerLight. This is unexpected and we\'d a" +
                "sk you to help us improve ServerLight by reporting this error.";
            // 
            // copyErrorCheckBox
            // 
            this.copyErrorCheckBox.Checked = true;
            this.copyErrorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copyErrorCheckBox.Location = new System.Drawing.Point(15, 360);
            this.copyErrorCheckBox.Name = "copyErrorCheckBox";
            this.copyErrorCheckBox.Size = new System.Drawing.Size(745, 24);
            this.copyErrorCheckBox.TabIndex = 2;
            this.copyErrorCheckBox.Text = "Copy error message to clipboard";
            // 
            // exceptionTextBox
            // 
            this.exceptionTextBox.Location = new System.Drawing.Point(0, 136);
            this.exceptionTextBox.Multiline = true;
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.ReadOnly = true;
            this.exceptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.exceptionTextBox.Size = new System.Drawing.Size(417, 222);
            this.exceptionTextBox.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.InitialImage")));
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(417, 98);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // reportButton
            // 
            this.reportButton.Location = new System.Drawing.Point(12, 390);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(206, 23);
            this.reportButton.TabIndex = 4;
            this.reportButton.Text = "Report Error";
            this.reportButton.Click += new System.EventHandler(this.reportErrorButtonClick);
            // 
            // ExceptionBox
            // 
            this.ClientSize = new System.Drawing.Size(423, 425);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.label);
            this.Controls.Add(this.reportButton);
            this.Controls.Add(this.copyErrorCheckBox);
            this.Controls.Add(this.exceptionTextBox);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionBox";
            this.Text = "Unhandled exception has occured";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button closeButton;

        private void buttonBedug_Click(object sender, EventArgs e)
        {
            Debugger.Break();
        }
    }
}

