using System;
using System.IO;
using System.Windows.Forms;

namespace StarDefendersLauncher.Dialogs
{
    public partial class EditProfileDialog : Form
    {
        string profile;
        public EditProfileDialog(string profile)
        {
            InitializeComponent();
            this.profile = profile;

            DialogResult = DialogResult.Cancel;
            namebox.Text = profile;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(namebox.Text) || FilePathHasInvalidChars(namebox.Text))
            {
                errorLabel.Text = "Invalid name";
                return;
            }

            if(Directory.Exists(Path.Combine(ProfilePicker.ProfilePath, namebox.Text)))
            {
                errorLabel.Text = "Profile already exists";
                return;
            }
            try
            {
                Directory.Move(Path.Combine(ProfilePicker.ProfilePath, profile), Path.Combine(ProfilePicker.ProfilePath, namebox.Text));
            }
            catch(ArgumentException)
            {
                errorLabel.Text = "Invalid name";
                return;
            }
            

            DialogResult = DialogResult.OK;

            Close();
        }

        public bool FilePathHasInvalidChars(string path)
        {
            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0);
        }
    }
}
