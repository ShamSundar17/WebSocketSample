using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Xamarin.Forms;
using Plugin.DeviceInfo;
using Newtonsoft.Json;

namespace SocketSampleShared
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime MessagDateTime { get; set; }

        public bool IsIncoming => UserId != CrossDeviceInfo.Current.Id;

        public string Name { get; set; }
        public string UserId { get; set; }
    }
    public class MyClass 
    {
        public static readonly ClientWebSocket client = new ClientWebSocket();
        public static readonly CancellationTokenSource cts = new CancellationTokenSource();
        public static readonly string username = "USER ONE";

        public IListener listener;

        public interface IListener {
            void OnListening();
        }

        public void SetOnListener(IListener l) {
            getListenerInfo().listener = l;
        }

        ListenerInfo getListenerInfo()
        {
            if (mListenerInfo != null)
            {
                return mListenerInfo;
            }
            mListenerInfo = new ListenerInfo();
            return mListenerInfo;
        }

        ListenerInfo mListenerInfo;

        ObservableCollection<Message> messages = new ObservableCollection<Message>();


        public MyClass()
        {
            Console.WriteLine("Inside Myclass Constructor:");
        }

        public async void ConnectToServerAsync()
        {
            Console.WriteLine("Inside ConnectToServerAsync");
            await client.ConnectAsync(new Uri("ws://echo.websocket.org"), cts.Token);


            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    WebSocketReceiveResult result;
                    var message = new ArraySegment<byte>(new byte[4096]);
                    do
                    {
                        result = await client.ReceiveAsync(message, cts.Token);
                        var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                        string serialisedMessae = Encoding.UTF8.GetString(messageBytes);
                        Console.WriteLine($"Inside ReceiveAsync. { serialisedMessae}");
                        //Console.WriteLine("Inside ReceiveAsync:", serialisedMessae);

                        try
                        {
                            var msg = JsonConvert.DeserializeObject<Message>(serialisedMessae);
                            messages.Add(msg);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Invalide message format. {ex.Message}");
                        }

                    } while (!result.EndOfMessage);
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);


        }
        public WebSocketState UpdateClientState()
        {
            Console.WriteLine("Inside UpdateClientState:");
            Console.WriteLine($"Websocket state: {client.State}");
            return client.State;
        }
        public async void SendMessageAsync(string message)
        {
            Console.WriteLine("Inside SendMessageAsync:");
            var msg = new Message
            {
                Name = username,
                MessagDateTime = DateTime.Now,
                Text = message,
                UserId = CrossDeviceInfo.Current.Id
            };

            string serialisedMessage = JsonConvert.SerializeObject(msg);

            var byteMessage = Encoding.UTF8.GetBytes(serialisedMessage);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
            //MessageText = string.Empty;
        }

        public void UpdatePlatform()
        {
            throw new NotImplementedException();
        }

        class ListenerInfo
        {
            public IListener listener;
        }
    }
}
