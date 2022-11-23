using CefSharp;
using System.Net;
using System;

namespace StarDefendersLauncher
{
    public class InternalSchemeFactory : ISchemeHandlerFactory
    {
        private readonly string schemeName;

        /// <summary>
        /// <see cref="ResourceHandler.GetMimeType(string)"/> is being deprecated in favour of using
        /// Chromiums native mimeType lookup which is accessible using Cef.GetMimeType, this method is however
        /// not directly available as it exists in CefSharp.Core, to get around this we set
        /// this static delegate with a reference to Cef.GetMimeType when Cef.Initialize is called.
        /// </summary>
        public static Func<string, string> GetMimeTypeDelegate = (s) => { return ResourceHandler.GetMimeType(s); };

        /// <summary>
        /// Initialize a new instance of FolderSchemeHandlerFactory
        /// </summary>
        /// <param name="rootFolder">Root Folder where all your files exist, requests cannot be made outside of this folder</param>
        /// <param name="schemeName">if not null then schemeName checking will be implemented</param>
        /// <param name="hostName">if not null then hostName checking will be implemented</param>
        /// <param name="defaultPage">default page if no page specified, defaults to index.html</param>
        /// <param name="resourceFileShare">file share mode used to open resources, defaults to FileShare.Read</param>
        public InternalSchemeFactory(string schemeName)
        {
            this.schemeName = schemeName;
        }

        /// <summary>
        /// If the file requested is within the rootFolder then a IResourceHandler reference to the file requested will be returned
        /// otherwise a 404 ResourceHandler will be returned.
        /// </summary>
        /// <param name="browser">the browser window that originated the
        /// request or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="frame">frame that originated the request
        /// or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="schemeName">the scheme name</param>
        /// <param name="request">The request. (will not contain cookie data)</param>
        /// <returns>
        /// A IResourceHandler
        /// </returns>
        IResourceHandler ISchemeHandlerFactory.Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return Create(browser, frame, schemeName, request);
        }

        /// <summary>
        /// If the file requested is within the rootFolder then a IResourceHandler reference to the file requested will be returned
        /// otherwise a 404 ResourceHandler will be returned.
        /// </summary>
        /// <param name="browser">the browser window that originated the
        /// request or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="frame">frame that originated the request
        /// or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="schemeName">the scheme name</param>
        /// <param name="request">The request. (will not contain cookie data)</param>
        /// <returns>
        /// A IResourceHandler
        /// </returns>
        protected virtual IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (this.schemeName != null && !schemeName.Equals(this.schemeName, StringComparison.OrdinalIgnoreCase))
            {
                return ResourceHandler.ForErrorMessage(string.Format("SchemeName {0} does not match the expected SchemeName of {1}.", schemeName, this.schemeName), HttpStatusCode.NotFound);
            }

            var baseUri = new Uri(request.Url.Substring(0, 11));
            var uri = new Uri(baseUri, request.Url.Substring(11));

            var asbolutePath = uri.AbsolutePath.Substring(1);
            char[] charsToTrim = { '/' };
            var absolutePath = asbolutePath.Trim(charsToTrim);

            if (absolutePath.EndsWith(".png"))
            {
                return ResourceHandler.FromStream(InternalHelper.getInternalStream(absolutePath), mimeType: "image/png");
            }
            else if (absolutePath.EndsWith(".css"))
            {
                return ResourceHandler.FromStream(InternalHelper.getInternalStream(absolutePath), mimeType: "text/css");
            }
            else
            {
                return ResourceHandler.FromStream(InternalHelper.getInternalStream(absolutePath));
            }
        }
    }
}