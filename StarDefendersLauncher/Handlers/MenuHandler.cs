using CefSharp;
using CefSharp.WinForms;
using System;

namespace StarDefendersLauncher.Handlers
{
    internal class MenuHandler : IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            if (model.Count > 0)
            {
                model.AddSeparator();
            }

            model.AddItem(CefMenuCommand.Back, "Back");
            model.AddItem(CefMenuCommand.Forward, "Forward");
            model.AddItem(CefMenuCommand.Reload, "Refresh");

            model.AddSeparator();

            model.AddItem(CefMenuCommand.Print, "Print");

            model.AddSeparator();

            model.AddItem((CefMenuCommand)26501, "Inspect element");
        }

        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if (commandId == (CefMenuCommand)26501)
            {
                browser.GetHost().ShowDevTools();
                return true;
            }

            if (commandId == (CefMenuCommand)26502)
            {
                browser.GetHost().CloseDevTools();
                return true;
            }

            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}