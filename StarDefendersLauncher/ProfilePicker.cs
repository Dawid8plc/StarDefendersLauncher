using StarDefendersLauncher.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StarDefendersLauncher
{
    public partial class ProfilePicker : Form
    {
        public static string ProfilePath = Path.Combine(Program.BasePath, "Profiles");

        public static ProfilePicker instance;

        public List<string> Profiles;

        public ProfilePicker()
        {
            InitializeComponent();
            instance = this;

            if (!Directory.Exists(ProfilePath))
            {
                Directory.CreateDirectory(ProfilePath);
            }
            else
            {
                RefreshList();
            }
        }

        public void RefreshList()
        {
            flowLayoutPanel1.Controls.Clear();

            Profiles = Directory.GetDirectories(ProfilePath).ToList();

            for (int i = 0; i < Profiles.Count; i++)
            {
                ProfileCard card = new ProfileCard(Path.GetFileName(Profiles[i]), i % 2 == 0);
                flowLayoutPanel1.Controls.Add(card);
            }
        }

        private void ProfilePicker_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms.Count == 0)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameForm form = new GameForm();
            form.Show();
            Close();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddProfileDialog dialog = new AddProfileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                RefreshList();
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.ShowDialog();
        }
    }
}
