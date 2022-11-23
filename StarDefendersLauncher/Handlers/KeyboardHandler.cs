using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace StarDefendersLauncher.Handlers
{
    internal class KeyboardHandler : IKeyboardHandler
    {
        GameForm form;

        bool isLoading;
        public KeyboardHandler(bool isLoading = false, GameForm form = null)
        {
            this.isLoading = isLoading;
            this.form = form;
        }

        public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            return true;
        }

        public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            if(type == KeyType.RawKeyDown && (Keys)windowsKeyCode == Keys.F11)
            {
                var cbrowser = (ChromiumWebBrowser)chromiumWebBrowser;
                var parent = (isLoading) ? cbrowser.Parent.Parent as Form : cbrowser.Parent as Form;
                cbrowser.InvokeOnUiThreadIfRequired(() =>
                {
                    if (parent.FormBorderStyle == FormBorderStyle.Sizable)
                    {
                        parent.FormBorderStyle = FormBorderStyle.None;
                        parent.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        parent.FormBorderStyle = FormBorderStyle.Sizable;
                        parent.WindowState = FormWindowState.Normal;
                    }
                });
            }

            if(!isLoading && type == KeyType.RawKeyDown && (Keys)windowsKeyCode == Keys.F5)
            {
                browser.Reload();
            }


            if (type == KeyType.RawKeyDown && (Keys)windowsKeyCode == Keys.F12)
            {
                browser.GetHost().ShowDevTools();

            }

            if (form != null && type == KeyType.RawKeyDown && (Keys)windowsKeyCode == Keys.Home)
            {
                form.ReturnLauncher();
            }

            return false;
        }
    }
}