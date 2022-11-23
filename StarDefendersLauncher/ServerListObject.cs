using StarDefendersLauncher.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarDefendersLauncher
{
    internal class ServerListObject
    {
        GameForm form;

        public ServerListObject(GameForm form)
        {
            this.form = form;
        }

        public List<Server> getServers()
        {
            return ServersManager.GetServers();
        }

        public void connect(string ip)
        {
            if (ip.StartsWith("localhost"))
            {
                form.HostandConnect(ip);
            }
            else
            {
                form.Connect(ip);
            }
        }

        public void quit()
        {
            form.Quit();
        }

        public void changeprofile()
        {
            form.ChangeProfile();
        }

        public bool addserver(string IP, string Location, string Desc)
        {
            IP = IP.Trim();
            if (string.IsNullOrWhiteSpace(IP))
                return false;

            ServersManager.UserList.Add(new Server() { IP = IP, Location = Location, Description = Desc, Type = "C" });
            ServersManager.Save();
            return true;
        }

        public void removeserver(int ID)
        {
            ID = ID - ServersManager.ServerList.Count;
            ServersManager.UserList.RemoveAt(ID);
            ServersManager.Save();
        }
    }
}
