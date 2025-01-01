using StarDefendersLauncher.Managers;
using System;
using System.IO;
using System.Windows.Forms;

namespace StarDefendersLauncher.Dialogs
{
    public partial class SettingsDialog : Form
    {
        
        public SettingsDialog()
        {
            InitializeComponent();
            
            DialogResult = DialogResult.Cancel;

            DRPBox.Checked = SettingsManager.Settings.RichPresenceEnabled;
            IgnoreLoadErrorsBox.Checked = SettingsManager.Settings.IgnoreLoadErrors;
            fullscreenBox.Checked = SettingsManager.Settings.Fullscreen;
            hideBIServersBox.Checked = SettingsManager.Settings.HideBuiltInServers;
            allowExpiredCertBox.Checked = SettingsManager.Settings.AllowExpiredCerts;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        public bool FilePathHasInvalidChars(string path)
        {
            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            SettingsManager.Settings.RichPresenceEnabled = DRPBox.Checked;
            SettingsManager.Settings.IgnoreLoadErrors = IgnoreLoadErrorsBox.Checked;
            SettingsManager.Settings.Fullscreen = fullscreenBox.Checked;
            SettingsManager.Settings.HideBuiltInServers = hideBIServersBox.Checked;
            SettingsManager.Settings.AllowExpiredCerts = allowExpiredCertBox.Checked;

            SettingsManager.Save();

            Close();
        }
    }
}
