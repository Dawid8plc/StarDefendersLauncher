using StarDefendersLauncher.Dialogs;
using System;
using System.Windows.Forms;

namespace StarDefendersLauncher
{
    public partial class ProfileCard : UserControl
    {
        string profile;
        public ProfileCard(string profile, bool even)
        {
            InitializeComponent();
            this.profile = profile;

            nameLabel.Text = profile;

            //Unused
            //if (!even)
            //{
            //    BackColor = Color.FromArgb(189, 195, 199);
            //}
            //else
            //{
            //    BackColor = Color.FromArgb(236, 240, 241);
            //}
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm(profile);
            gameForm.Show();
            ProfilePicker.instance.Close();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            RemoveProfileDialog dialog = new RemoveProfileDialog(profile);

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                ProfilePicker.instance.RefreshList();
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            EditProfileDialog dialog = new EditProfileDialog(profile);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ProfilePicker.instance.RefreshList();
            }
        }
    }
}
