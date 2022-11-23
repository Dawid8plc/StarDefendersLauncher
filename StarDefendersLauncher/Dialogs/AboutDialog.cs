using System;
using System.IO;
using System.Windows.Forms;

namespace StarDefendersLauncher.Dialogs
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
