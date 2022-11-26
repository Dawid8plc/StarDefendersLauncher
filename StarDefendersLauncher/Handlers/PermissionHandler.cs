using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarDefendersLauncher.Handlers
{
    internal class PermissionHandler : IPermissionHandler
    {
        public void OnDismissPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, PermissionRequestResult result)
        {
            
        }

        public bool OnRequestMediaAccessPermission(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string requestingOrigin, MediaAccessPermissionType requestedPermissions, IMediaAccessCallback callback)
        {
            return false;
        }

        public bool OnShowPermissionPrompt(IWebBrowser chromiumWebBrowser, IBrowser browser, ulong promptId, string requestingOrigin, PermissionRequestType requestedPermissions, IPermissionPromptCallback callback)
        {
            if (requestedPermissions == PermissionRequestType.Clipboard)
            {
                callback.Continue(PermissionRequestResult.Accept);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
