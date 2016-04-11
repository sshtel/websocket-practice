using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebsocketClient
{
    public partial class FormWebSocketClient : Form
    {
        public FormWebSocketClient()
        {
            InitializeComponent();
        }

        private void FormWebSocketClient_Load(object sender, EventArgs e)
        {

        }


        WebSocketClientThread socketClientObject;
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            String uri = this.textBoxServerAddress.Text;
            
            socketClientObject = new WebSocketClientThread(uri, this);
            
            
            Thread clientThread = new Thread(socketClientObject.DoWork);
            clientThread.Start();

        }


        public void updateReceivedMsg(String msg)
        {
            if (this.textBox1.InvokeRequired)
            {
                WebSocketClientThread.ReceivedMsgDelegate d = new WebSocketClientThread.ReceivedMsgDelegate(updateReceivedMsg);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.textBox1.AppendText(msg);
            }

        }


        public void updateStatus(String msg)
        {
            if (this.textBoxStatus.InvokeRequired)
            {
                WebSocketClientThread.ReceivedMsgDelegate d = new WebSocketClientThread.ReceivedMsgDelegate(updateReceivedMsg);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.textBox1.AppendText(msg);
            }
        }

        private void buttonSendMsg_Click(object sender, EventArgs e)
        {
            
            String msg = this.textBoxMyMsg.Text;
            socketClientObject.SendFromClient(msg);
        }
        

    }
}
