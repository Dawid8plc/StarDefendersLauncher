using System.IO;
using System.Xml.Serialization;

namespace StarDefendersLauncher.Managers
{
    public class SettingsManager
    {
        static string SettingsPath = Path.Combine(Program.BasePath, "Star Defenders Launcher settings.xml");
        static XmlSerializer serializer;

        public static Settings Settings;

        public static void Initialize()
        {
            serializer = new XmlSerializer(typeof(Settings));

            if (File.Exists(SettingsPath))
            {
                Load();
            }
            else
            {
                Settings = new Settings();
                Save();
            }
        }

        public static void Save()
        {
            FileStream stream = File.Create(SettingsPath);
            serializer.Serialize(stream, Settings);
            stream.Close();
        }

        public static void Load()
        {
            FileStream stream = File.OpenRead(SettingsPath);
            Settings = (Settings)serializer.Deserialize(stream);
            stream.Close();
        }
    }

    public class Settings
    {
        public bool RichPresenceEnabled = true;
        public bool IgnoreLoadErrors = false;
        public bool Fullscreen = false;
        public bool HideBuiltInServers = false;
        public bool AllowExpiredCerts = false;
    }
}
