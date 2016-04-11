using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebsocketClient
{
    class WebSocketClientThread
    {
        private bool isWorking_ = true;

        private String uri_ = "";
        public void setUri(String uri) { this.uri_ = uri; }

        FormWebSocketClient form_;

        public WebSocketClientThread(String uri, FormWebSocketClient form)
        {
            this.uri_ = uri;
            this.form_ = (FormWebSocketClient)form;
        }


        public void setFormRef(FormWebSocketClient form) { this.form_ = form;  }

        public void DoWork() { 

            while(isWorking_){
                form_.updateReceivedMsg("connecting!");
                if (form_ != null) {
                    Connect(uri_).Wait();
                }                
                Thread.Sleep(5000);
            }
        }
        
        private static object consoleLock = new object();
        private const int sendChunkSize = 256;
        private const int receiveChunkSize = 256;
        private const bool verbose = true;
        private static readonly TimeSpan delay = TimeSpan.FromMilliseconds(1000);


        ClientWebSocket webSocket = null;
        public async Task Connect(string uri)
        {
            try
            {
                
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                form_.updateStatus("Connected");
                Task[] tasks = { Receive(webSocket), Send(webSocket) };
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
                Console.WriteLine();

                lock (consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WebSocket closed.");
                    Console.ResetColor();
                }
            }
        }

        public async void Close() {
            await webSocket.CloseAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
        }

        static UTF8Encoding encoder = new UTF8Encoding();


        public async void SendFromClient(String msg)
        {
            if (webSocket.State == WebSocketState.Open) { 
                byte []byteStr = Encoding.UTF8.GetBytes(msg);
                var segment = new ArraySegment<byte>(byteStr);
                await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task Send(ClientWebSocket webSocket)
        {

            //byte[] buffer = encoder.GetBytes("{\"op\":\"blocks_sub\"}"); //"{\"op\":\"unconfirmed_sub\"}");
            //byte[] buffer = encoder.GetBytes("{\"op\":\"unconfirmed_sub\"}");
            //await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

            while (webSocket.State == WebSocketState.Open)
            {
                //LogStatus(false, buffer, buffer.Length);
                await Task.Delay(delay);
            }
        }

        public delegate void ReceivedMsgDelegate(String msg);

        
        private async Task Receive(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[receiveChunkSize];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    String msg = Encoding.UTF8.GetString(buffer);
                    form_.updateReceivedMsg(msg);
                    form_.updateReceivedMsg("\n");
                    Array.Clear(buffer, 0, buffer.Length);
                }
            }
        }

        public void stop() {
            isWorking_ = false;
        }
        
    }
}
