using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StarDefendersLauncher.Managers
{
    public class SkinsManager
    {
        static string SkinsPath = Path.Combine(Program.BasePath, "Star Defenders Launcher skins.xml");
        static XmlSerializer serializer;

        public static List<Skin> Skins;

        public static void Initialize()
        {
            serializer = new XmlSerializer(typeof(List<Skin>));

            if (File.Exists(SkinsPath))
            {
                Load();
            }
            else
            {
                Skins = new List<Skin>();
                Save();
            }
        }

        public static void Save()
        {
            FileStream stream = File.Create(SkinsPath);
            serializer.Serialize(stream, Skins);
            stream.Close();
        }

        public static void Load()
        {
            FileStream stream = File.OpenRead(SkinsPath);
            Skins = (List<Skin>)serializer.Deserialize(stream);
            stream.Close();
        }

        public async static Task ExportSkin(ChromiumWebBrowser browser, string name)
        {
            var script = @"function test (){ 
                                    var data = JSON.stringify(localStorage);
                                    data = JSON.parse(data);

                                    delete data.my_hash;
                                    delete data.my_net_id;

                                    data = JSON.stringify(data);
                                    return data;
                                    }
                                test();"
            ;

            var result = await browser.GetMainFrame().EvaluateScriptAsync(script).ContinueWith(v =>
            {
                var res = v.Result.Result;

                return (string)res;
            });

            Skins.Add(new Skin()
            {
                Name = name,
                Data = result,
                IP = browser.Address.TrimEnd(new char[]{'/'})
            });

            Save();
        }

        public async static Task ImportSkin(ChromiumWebBrowser browser, Skin skin)
        {
            var script = $@"function test (){{ 
                                    var data = JSON.parse('{skin.Data}');
                                    Object.keys(data).forEach(function(k){{ localStorage.setItem(k, data[k]); }});

                                    return true;
                                    }}
                                test();"
;

            var result = await browser.GetMainFrame().EvaluateScriptAsync(script).ContinueWith(v =>
            {
                return (bool)v.Result.Result;
            });
        }
    }

    public class Skin
    {
        public string Name;
        public string IP;
        public string Data;
    }
}
