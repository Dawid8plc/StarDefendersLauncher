using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using WebSocketSharp;

namespace StarDefendersLauncher
{
    public partial class NodeSocketTerminal : Form
    {
        int commandId = 0;
        Dictionary<int, string> RanCommands = new Dictionary<int, string>();

        bool connected = false;

        WebSocket ws;

        public NodeSocketTerminal()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            ws.Close();
            Close();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                connectBtn.Enabled = false;
                ws.Close();
            }
            else
            {
                connectBtn.Enabled = false;

                string inputUrl = debuggerUrl.Text.Trim();
                string websocketUrl = string.Empty;

                if (!inputUrl.StartsWith("ws://"))
                {
                    string jsonInfo;

                    try
                    {
                        using (WebClient wc = new WebClient())
                        {
                            jsonInfo = wc.DownloadString("http://" + inputUrl + "/json");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Failed to connect");
                        connectBtn.Enabled = true;
                        return;
                    }

                    dynamic info = JsonConvert.DeserializeObject<dynamic>(jsonInfo)[0];

                    try
                    {
                        websocketUrl = info.webSocketDebuggerUrl.Value;
                    }
                    catch
                    {
                        MessageBox.Show("Failed to extract websocket url");
                        connectBtn.Enabled = true;
                        return;
                    }
                }
                else
                {
                    websocketUrl = inputUrl;
                }

                ws = new WebSocket(websocketUrl);

                ws.OnMessage += Ws_OnMessage;
                ws.OnOpen += Ws_OnOpen;
                ws.OnClose += Ws_OnClose;
                ws.OnError += Ws_OnError;

                ws.Connect();
            }
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            connected = false;

            //This has to be invoked on the UI Thread in case the server closes connection, which appears to be calling this event
            //on a different thread (doesn't happen if we are the one to disconnect, though)
            connectBtn.InvokeOnUiThreadIfRequired(() =>
            {
                connectBtn.Enabled = true;
                sendBtn.Enabled = false;
                commandBox.Enabled = false;

                connectBtn.Text = "Connect";
                statusLabel.Text = "Status: Disconnected";

                commandId = 0;
                RanCommands.Clear();
            });
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            connected = true;
            jsonNodesView.Nodes.Clear();

            connectBtn.Enabled = true;
            sendBtn.Enabled = true;
            commandBox.Enabled = true;

            connectBtn.Text = "Disconnect";
            statusLabel.Text = "Status: Connected";
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            JObject obj = JObject.Parse(e.Data);

            jsonNodesView.InvokeOnUiThreadIfRequired(() =>
            {
                int ID = (int)obj["id"];

                string ranCommand = RanCommands[ID];

                TreeNode parent = Json2Tree(obj);
                parent.Text = ranCommand;
                jsonNodesView.Nodes.Add(parent);

                jsonNodesView.Nodes[jsonNodesView.Nodes.Count - 1].EnsureVisible();
            });
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (!connected || string.IsNullOrWhiteSpace(commandBox.Text))
                return;

            string jsCode = commandBox.Text;
            string evaluateCommand = $"{{ \"id\": {commandId}, \"method\": \"Runtime.evaluate\", \"params\": {{ \"expression\": \"{jsCode}\", \"contextId\": 1 }} }}";

            RanCommands.Add(commandId, commandBox.Text);
            commandId++;

            ws.Send(evaluateCommand);

            commandBox.Text = string.Empty;
        }

        private void NodeSocketTerminal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms.Count == 0)
                Application.Exit();
        }

        private TreeNode Json2Tree(JObject obj)
        {
            //create the parent node
            TreeNode parent = new TreeNode();
            //loop through the obj. all token should be pair<key, value>
            foreach (var token in obj)
            {
                //change the display Content of the parent
                parent.Text = token.Key.ToString();
                //create the child node
                TreeNode child = new TreeNode();
                child.Text = token.Key.ToString();
                //check if the value is of type obj recall the method
                if (token.Value.Type.ToString() == "Object")
                {
                    // child.Text = token.Key.ToString();
                    //create a new JObject using the the Token.value
                    JObject o = (JObject)token.Value;
                    //recall the method
                    child = Json2Tree(o);
                    //add the child to the parentNode
                    parent.Nodes.Add(child);
                }
                //if type is of array
                else if (token.Value.Type.ToString() == "Array")
                {
                    int ix = -1;
                    //  child.Text = token.Key.ToString();
                    //loop though the array
                    foreach (var itm in token.Value)
                    {
                        //check if value is an Array of objects
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();
                            //child.Text = token.Key.ToString();
                            //call back the method
                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = token.Key.ToString() + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                            //parent.Nodes.Add(child);
                        }
                        //regular array string, int, etc
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = token.Key.ToString() + "[" + ix + "]";
                                dataArray.Nodes.Add(data.ToString());
                            }
                            child.Nodes.Add(dataArray);
                        }

                        else
                        {
                            child.Nodes.Add(itm.ToString());
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    //if token.Value is not nested
                    // child.Text = token.Key.ToString();
                    //change the value into N/A if value == null or an empty string 
                    if (token.Value.ToString() == "")
                        child.Nodes.Add("N/A");
                    else
                        child.Nodes.Add(token.Value.ToString());
                    parent.Nodes.Add(child);
                }
            }
            return parent;

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jsonNodesView.Nodes.Clear();
        }

        private void commandBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendBtn.PerformClick();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(jsonNodesView.SelectedNode != null)
                Clipboard.SetText(jsonNodesView.SelectedNode.Text);
        }
    }
}
