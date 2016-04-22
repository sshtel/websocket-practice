using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayTag
{
    public partial class FormDayTagClient : Form
    {
        DayWebSocketClient wsClient = new DayWebSocketClient();
        public FormDayTagClient()
        {
            InitializeComponent();
            wsClient.setForm(this);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (wsClient.isAlive())
            {
                this.buttonConnect.Text = "connect";
                wsClient.disconnect();
            }
            else {
                wsClient.connect();
                this.buttonConnect.Text = "disconnect";
            }
            
        }

        public void updateMessage(String msg) {
            if (this.textBoxMain.InvokeRequired)
            {
                DayWebSocketClient.ReceivedMsgDelegate d = new DayWebSocketClient.ReceivedMsgDelegate(updateMessage);
                this.Invoke(d, new object[] { msg });
            }
            else {
                this.textBoxMain.AppendText(msg);
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (wsClient.isAlive()) {
                wsClient.sendMsg(textBoxMyMsg.Text);
            }
        }

    }
}
