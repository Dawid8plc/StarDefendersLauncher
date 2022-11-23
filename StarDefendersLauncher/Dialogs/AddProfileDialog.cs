using System;
using System.IO;
using System.Windows.Forms;

namespace StarDefendersLauncher.Dialogs
{
    public partial class AddProfileDialog : Form
    {
        public AddProfileDialog()
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;
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
                Directory.CreateDirectory(Path.Combine(ProfilePicker.ProfilePath, namebox.Text));
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
