using System.IO;
using System.Reflection;

namespace StarDefendersLauncher
{
    public static class InternalHelper
    {
        public static Stream getInternalStream(string page)
        {
            var absolutePath = page.Replace('/', '.');
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName;

            if (page.Contains("."))
            {
                resourceName = "StarDefendersLauncher." + absolutePath;
            }
            else
            {
                resourceName = "StarDefendersLauncher." + absolutePath + ".html";
            }

            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return stream;
        }
    }
}
