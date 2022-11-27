using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace StarDefendersLauncher.Managers
{
    public class ServersManager
    {
        public static List<Server> ServerList = new List<Server>();
        public static List<Server> UserList;

        static string ServersPath = Path.Combine(Program.BasePath, "Star Defenders Launcher servers.xml");
        static XmlSerializer serializer;

        public static void Initialize()
        {
            //Hardcoded built in servers
            ServerList.Add(new Server() { IP = "localhost:3000", Type = "A", Location = "Your local server", Description = "", BuiltIn = true });
            ServerList.Add(new Server() { IP = "https://www.gevanni.com:3000", Type = "B", Location = "Europe, Ukraine", Description = "Main test server, core game in its' raw unchanged state", BuiltIn = true });
            ServerList.Add(new Server() { IP = "https://www.plazmaburst2.com:8443", Type = "B", Location = "USA, California", Description = "Secondary server, no rules, only blue base shielding units", BuiltIn = true });
            ServerList.Add(new Server() { IP = "https://stardefenders.io:3000", Type = "C", Location = "Canada, Quebec", Description = "Active development<br>Hosted by mrmcshroom -- ULTRAWIDE", BuiltIn = true });
            ServerList.Add(new Server() { IP = "https://stardefenders.io:3001", Type = "C", Location = "Canada, Quebec", Description = "Active development<br>Hosted by mrmcshroom -- EASY", BuiltIn = true });
            ServerList.Add(new Server() { IP = "https://stardefenders.io:3002", Type = "C", Location = "Canada, Quebec", Description = "Active development<br>Hosted by mrmcshroom -- PEACEFUL", BuiltIn = true });
            ServerList.Add(new Server() { IP = "https://stardefenders.io:4000", Type = "C", Location = "Canada, Quebec", Description = "Active development<br>Hosted by mrmcshroom, managed by Booraz.", BuiltIn = true });
            //ServerList.Add(new Server() { IP = "https://linstardefenders.live", Type = "C", Location = "Hong Kong", Description = "Active development<br>Hosted by Undriven", BuiltIn = true });

            serializer = new XmlSerializer(typeof(List<Server>));

            //User's servers
            if (File.Exists(ServersPath))
            {
                Load();
            }
            else
            {
                UserList = new List<Server>();
                Save();
            }
        }

        public static void Save()
        {
            FileStream stream = File.Create(ServersPath);
            serializer.Serialize(stream, UserList);
            stream.Close();
        }

        public static void Load()
        {
            FileStream stream = File.OpenRead(ServersPath);
            UserList = (List<Server>)serializer.Deserialize(stream);
            stream.Close();
        }

        internal static List<Server> GetServers()
        {
            List<Server> allServers;
            if (!SettingsManager.Settings.HideBuiltInServers)
            {
                allServers = new List<Server>(ServerList.Count + UserList.Count);
                allServers.AddRange(ServerList); allServers.AddRange(UserList);
            }
            else
            {
                allServers = new List<Server>(UserList.Count + 1);

                //localhost is a special case, so I think it should stay
                allServers.Add(new Server() { IP = "localhost:3000", Type = "A", Location = "Your local server", Description = "", BuiltIn = true });

                allServers.AddRange(UserList);
            }

            return allServers;
        }
    }

    public class Server
    {
        public string Location;
        public string IP;
        public string Description;
        public string Type;
        public bool BuiltIn;
    }
}
