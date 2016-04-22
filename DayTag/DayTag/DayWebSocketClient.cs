using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;


namespace DayTag
{
    class DayWebSocketClient
    {
        private FormDayTagClient form_;
        public void setForm(FormDayTagClient form) { form_ = form;  }

        public delegate void ReceivedMsgDelegate(String msg);

        WebSocketSharp.WebSocket webSocket = new WebSocketSharp.WebSocket("ws://localhost:8084/summer/websocket/chat.do");

        public DayWebSocketClient() {
            {
                // Set the WebSocket events.

                webSocket.OnOpen += (sender, e) => webSocket.Send("Hi, there!");

                webSocket.OnMessage += (sender, e) =>
                {
                    var body = !e.IsPing ? e.Data : "Received a ping.";
                    Console.WriteLine(body);
                    form_.updateMessage(body + "\n");
                };

                webSocket.OnError += (sender, e) =>
                {
                    Console.WriteLine(e.Message);
                    form_.updateMessage(e.Message);
                };

                webSocket.OnClose += (sender, e) =>
                {
                    Console.WriteLine(e.Reason);
                    form_.updateMessage(e.Reason);
                };
#if DEBUG
                // To change the logging level.
                webSocket.Log.Level = LogLevel.Trace;

                // To change the wait time for the response to the Ping or Close.
                webSocket.WaitTime = TimeSpan.FromSeconds(10);

                // To emit a WebSocket.OnMessage event when receives a ping.
                webSocket.EmitOnPing = true;
#endif
                // To enable the Per-message Compression extension.

                
            }
        }

        public void connect()
        {
            // Connect to the server.
            if(webSocket != null)  webSocket.Connect();
        }
        public void disconnect() {
            if (webSocket != null)  webSocket.Close();
        }

        public bool isAlive() {
            if(webSocket != null) return webSocket.IsAlive;
            return false;
        }

        public void sendMsg(String msg) {
//            byte[] StrByte = Encoding.UTF8.GetBytes(msg);
            webSocket.Send(msg);
        }
        

    }
}
