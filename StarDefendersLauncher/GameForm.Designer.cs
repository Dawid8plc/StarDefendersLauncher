namespace StarDefendersLauncher
{
    partial class GameForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.browser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.LoadingPanel = new System.Windows.Forms.Panel();
            this.LoadingPanel2 = new System.Windows.Forms.Panel();
            this.loadingBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.LoadingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.ActivateBrowserOnCreation = false;
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(1264, 681);
            this.browser.TabIndex = 0;
            // 
            // LoadingPanel
            // 
            this.LoadingPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LoadingPanel.Controls.Add(this.LoadingPanel2);
            this.LoadingPanel.Controls.Add(this.loadingBrowser);
            this.LoadingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingPanel.Location = new System.Drawing.Point(0, 0);
            this.LoadingPanel.Name = "LoadingPanel";
            this.LoadingPanel.Size = new System.Drawing.Size(1264, 681);
            this.LoadingPanel.TabIndex = 1;
            // 
            // LoadingPanel2
            // 
            this.LoadingPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingPanel2.Location = new System.Drawing.Point(0, 0);
            this.LoadingPanel2.Name = "LoadingPanel2";
            this.LoadingPanel2.Size = new System.Drawing.Size(1264, 681);
            this.LoadingPanel2.TabIndex = 1;
            // 
            // loadingBrowser
            // 
            this.loadingBrowser.ActivateBrowserOnCreation = false;
            this.loadingBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadingBrowser.Location = new System.Drawing.Point(0, 0);
            this.loadingBrowser.Name = "loadingBrowser";
            this.loadingBrowser.Size = new System.Drawing.Size(1264, 681);
            this.loadingBrowser.TabIndex = 0;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.LoadingPanel);
            this.Controls.Add(this.browser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Star Defenders";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameForm_FormClosed);
            this.LoadingPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser browser;
        private System.Windows.Forms.Panel LoadingPanel;
        private CefSharp.WinForms.ChromiumWebBrowser loadingBrowser;
        private System.Windows.Forms.Panel LoadingPanel2;
    }
}

