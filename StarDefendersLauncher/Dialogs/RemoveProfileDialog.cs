using System;
using System.IO;
using System.Windows.Forms;

namespace StarDefendersLauncher.Dialogs
{
    public partial class RemoveProfileDialog : Form
    {
        string profile;
        public RemoveProfileDialog(string profile)
        {
            InitializeComponent();
            this.profile = profile;

            DialogResult = DialogResult.Cancel;

            questionLabel.Text = $"Are you sure you want to remove the \"{profile}\" profile?";
        }

        private void noBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void yesBtn_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(Path.Combine(ProfilePicker.ProfilePath, profile)))
                Directory.Delete(Path.Combine(ProfilePicker.ProfilePath, profile), true);

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
