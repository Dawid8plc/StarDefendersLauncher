using DiscordRPC;
using DiscordRPC.Logging;
using StarDefendersLauncher.Managers;
using System;

namespace StarDefendersLauncher.Managers
{
    internal class DiscordManager
    {
        public DiscordRpcClient client;

        public DiscordManager()
        {
            /*
	        Create a Discord client
	        NOTE: 	If you are using Unity3D, you must use the full constructor and define
			         the pipe connection.
	        */
            client = new DiscordRpcClient("1043953089801101355");

            //Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            //Connect to the RPC
            client.Initialize();

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            SetPresence(new RichPresence()
            {
                Details = "In Launcher",
                Assets = new Assets()
                {
                    LargeImageKey = "icon",
                }
            });
        }

        public void SetPresence(RichPresence presence)
        {
            if(SettingsManager.Settings.RichPresenceEnabled)
                client.SetPresence(presence);
        }

        public void Deinitialize()
        {
            client.Dispose();
        }
    }
}