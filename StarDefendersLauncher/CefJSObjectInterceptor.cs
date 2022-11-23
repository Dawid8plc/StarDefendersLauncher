using CefSharp.ModelBinding;
using CefSharp.WinForms;
using CefSharp;
using System;

namespace StarDefendersLauncher
{
    internal class CefJSObjectInterceptor : IMethodInterceptor
    {
        string currentAddress;

        public CefJSObjectInterceptor(ChromiumWebBrowser browser)
        {
            browser.AddressChanged += Browser_AddressChanged;
        }

        public object Intercept(Func<object[], object> method, object[] parameters, string methodName)
        {
            if (isAcceptableURL(currentAddress))
            {
                object result = method(parameters);
                return result;
            }
            return null;
        }

        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            currentAddress = e.Address;
        }

        bool isAcceptableURL(string url)
        {
            if (url.StartsWith("launcher"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
