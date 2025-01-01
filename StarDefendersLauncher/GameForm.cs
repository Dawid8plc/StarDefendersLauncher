using CefSharp;
using CefSharp.WinForms;
using DiscordRPC;
using StarDefendersLauncher.Handlers;
using StarDefendersLauncher.Managers;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace StarDefendersLauncher
{
    public partial class GameForm : Form
    {
        public LauncherState state = LauncherState.MainMenu;

        //bool isPlaying = false;

        DiscordManager DM;

        bool isConnecting = false;

        public GameForm(string profile = "Default")
        {
            var settings = new CefSettings()
            {
                CachePath = Path.Combine(ProfilePicker.ProfilePath, profile)
            };

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "launcher",
                SchemeHandlerFactory = new InternalSchemeFactory("launcher")
            });

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            InitializeComponent();

            if (SettingsManager.Settings.Fullscreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
            }

            DM = new DiscordManager();

            BrowserSettings BS = new BrowserSettings();
            BS.JavascriptAccessClipboard = CefState.Enabled;

            browser.BrowserSettings = BS;
            browser.KeyboardHandler = new KeyboardHandler(form: this);
            browser.MenuHandler = new MenuHandler();
            browser.PermissionHandler = new PermissionHandler();
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
            browser.LoadError += Browser_LoadError;
            browser.RequestHandler = new CustomRequestHandler();

            loadingBrowser.LoadingStateChanged += LoadingBrowser_LoadingStateChanged;
            loadingBrowser.KeyboardHandler = new KeyboardHandler(true);

            BindingOptions bindingOptions = new BindingOptions();
            bindingOptions.MethodInterceptor = new CefJSObjectInterceptor(loadingBrowser);

            loadingBrowser.JavascriptObjectRepository.Register("ServerListObject", new ServerListObject(this), true, bindingOptions);

            loadingBrowser.Load("launcher://Loading.html");
        }

        private void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            if (SettingsManager.Settings.IgnoreLoadErrors) return;

            if (isConnecting)
            {
                Invoke(new Action(() =>
                {
                    browser.Load("about:blank");
                    //make sure there's only one event handler
                    browser.LoadingStateChanged -= Browser_LoadingStateChanged;
                    browser.LoadingStateChanged += Browser_LoadingStateChanged;
                    loadingBrowser.Load("launcher://Error.html");
                    LoadingPanel.Visible = true;
                    //isPlaying = false;
                    state = LauncherState.MainMenu;

                    DM.SetPresence(new RichPresence()
                    {
                        Details = "In Launcher",
                        Assets = new Assets()
                        {
                            LargeImageKey = "icon",
                        }
                    });
                }));
            }
        }

        private void LoadingBrowser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                Invoke(new Action(() =>
                {
                    //LoadingPanel.Controls.Remove(LoadingPanel2);
                    LoadingPanel2.Visible = false;
                    loadingBrowser.LoadingStateChanged -= LoadingBrowser_LoadingStateChanged;
                }));

            }
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading && browser.Address != "about:blank")
            {
                if (state == LauncherState.MainMenu)
                {
                    Invoke(new Action(() =>
                    {
                        //Controls.Remove(LoadingPanel);
                        loadingBrowser.Load("launcher://Loading.html");
                        LoadingPanel.Visible = false;
                        browser.LoadingStateChanged -= Browser_LoadingStateChanged;
                        //isPlaying = true; 
                        state = LauncherState.Playing;
                        isConnecting = false;
                    }));
                }
                else if (state == LauncherState.Exporting)
                {
                    Invoke(new Action(async () =>
                    {
                        await SkinsManager.ExportSkin(browser, toExport);

                        browser.Load("about:blank");
                        state = LauncherState.MainMenu;
                        isConnecting = false;
                        toExport = null;

                        var script = @"ExportDone();";

                        loadingBrowser.GetMainFrame().ExecuteJavaScriptAsync(script);
                    }));
                }else if(state == LauncherState.Importing)
                {
                    Invoke(new Action(async () =>
                    {
                        await SkinsManager.ImportSkin(browser, toImport);

                        browser.Load("about:blank");
                        state = LauncherState.MainMenu;
                        isConnecting = false;
                        toImport = null;

                        var script = @"ExportDone();";

                        loadingBrowser.GetMainFrame().ExecuteJavaScriptAsync(script);
                    }));
                }

            }
        }

        private void Browser_TitleChanged(object sender, CefSharp.TitleChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (!IsDisposed)
                    Text = e.Title;
            }));
        }

        internal void Connect(string ip)
        {
            isConnecting = true;
            browser.Load(ip);

            DM.SetPresence(new RichPresence()
            {
                Details = "Playing",
                Assets = new Assets()
                {
                    LargeImageKey = "icon",
                    LargeImageText = ip
                }
            });
        }

        Process server;
        string ServerIP;

        internal void HostandConnect(string ip)
        {
            if (Directory.Exists(Path.Combine(Program.BasePath, "Game")))
            {
                string GamePath = Path.Combine(Program.BasePath, "Game");
                if (File.Exists(Path.Combine(GamePath, "nodejs/node.exe")) && File.Exists(Path.Combine(GamePath, "index.js"))) {
                    ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(GamePath, "nodejs/node.exe"), "--inspect index.js");
                    psi.WorkingDirectory = GamePath;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardError = true;
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;

                    server = new Process();
                    server.StartInfo = psi;
                    server.OutputDataReceived += Server_OutputDataReceived;
                    server.EnableRaisingEvents = true;
                    ServerIP = ip;

                    server.Start();
                    server.BeginOutputReadLine();
                }
            }
            else
            {
                isConnecting = true;
                browser.Load(ip);

                DM.SetPresence(new RichPresence()
                {
                    Details = "Playing",
                    Assets = new Assets()
                    {
                        LargeImageKey = "icon",
                        LargeImageText = ip
                    }
                });
            }
        }

        private void Server_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(e.Data != null && e.Data.StartsWith("listening on"))
            {
                isConnecting = true;
                browser.Load(ServerIP);

                DM.SetPresence(new RichPresence()
                {
                    Details = "Playing",
                    Assets = new Assets()
                    {
                        LargeImageKey = "icon",
                        LargeImageText = ServerIP
                    }
                });
            }
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DM.Deinitialize();

            if (server != null && !server.HasExited)
            {
                server.CancelOutputRead();
                server.Kill();
            }

            if (Application.OpenForms.Count == 0)
                Application.Exit();
        }

        internal void ReturnLauncher()
        {
            if (state == LauncherState.Playing || state == LauncherState.Exporting || state == LauncherState.Importing)
            {
                if(server != null && !server.HasExited)
                {
                    server.CancelOutputRead();
                    server.Kill();
                    server = null;
                }

                Invoke(new Action(() =>
                {
                    browser.Load("about:blank");
                    
                    browser.LoadingStateChanged += Browser_LoadingStateChanged;
                    LoadingPanel.Visible = true;
                    //isPlaying = false;
                    state = LauncherState.MainMenu;
                    isConnecting = false;

                    DM.SetPresence(new RichPresence()
                    {
                        Details = "In Launcher",
                        Assets = new Assets()
                        {
                            LargeImageKey = "icon",
                        }
                    });
                }));
            }
        }

        internal void Quit()
        {
            Invoke(new Action(() =>
            {
                Close();
            }));
        }

        internal void ChangeProfile()
        {
            Invoke(new Action(() =>
            {
                //Calling Cef.Initialize a second time is not allowed - even if you call Shutdown before doing that. Because of that
                //its impossible to set the cache path using Initialize to a different profile, so restarting the application it is
                //Could probably play around with RequestContext but maybe in the future
                //ProfilePicker picker = new ProfilePicker();
                //picker.Show();

                //Close();

                //Cef.Shutdown();

                Application.Restart();
                Environment.Exit(0);
            }));
        }

        internal void ExportData(string IP, string Name)
        {
            isConnecting = true;
            toExport = Name;
            state = LauncherState.Exporting;
            browser.Load(IP);
        }

        Skin toImport;
        string toExport;

        internal void ImportData(string IP, Skin skin)
        {
            isConnecting = true;
            state = LauncherState.Importing;
            toImport = skin;
            browser.Load(IP);
        }
    }

    public enum LauncherState
    {
        MainMenu,
        Playing,
        Exporting,
        Importing
    }
}
