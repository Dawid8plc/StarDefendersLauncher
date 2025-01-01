using CefSharp;
using CefSharp.Handler;
using StarDefendersLauncher.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarDefendersLauncher.Handlers
{
    internal class CustomRequestHandler : RequestHandler
    {
        protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            if (SettingsManager.Settings.AllowExpiredCerts && errorCode == CefErrorCode.CertDateInvalid)
            {
                callback.Continue(true);
                return true;
            }
            else
            {
                return base.OnCertificateError(chromiumWebBrowser, browser, errorCode, requestUrl, sslInfo, callback);
            }
        }
    }
}
