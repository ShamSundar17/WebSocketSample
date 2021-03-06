﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using SocketSampleShared.Helper;
using SocketSampleShared.Models;

#if __IOS__
    using WebSocketsSample.iOS;
#else
    using WebSocketsSample.Droid;
#endif

namespace SocketSampleShared
{
    public class SocketImpl : ISocket 
    {
        public readonly ClientWebSocket client ;
        public readonly CancellationTokenSource cts ;
        public ObservableCollection<Models.Messages> messages = new ObservableCollection<Models.Messages>();
        public IListener listener;

#if __IOS__
        public SocketImpl(SecondPage viewController)
        {
            this.listener = viewController;
            this.client = new ClientWebSocket();
            this.cts = new CancellationTokenSource();
        }
#else
        public SocketImpl(MainActivity activity)
        {
            this.listener = activity;
            this.client = new ClientWebSocket();
            this.cts = new CancellationTokenSource();
        }
#endif


        public async void ConnectToServerAsync()
        {
            switch (GetSocketState().ToString())
            {
                case "None":
                    Console.WriteLine("ConnectToServerAsync Websocket state : None");
                    await client.ConnectAsync(new Uri(Constants.SocketUrl), cts.Token);
                    ReceiveMessageAsync();
                    break;
                case "Open":
                    Console.WriteLine("Websocket state : Open , ConnectToServerAsync");
                    ReceiveMessageAsync();
                    break;
                case "Connecting":
                    Console.WriteLine("Websocket state : Connecting");
                    break;
                case "CloseSent":
                    Console.WriteLine("Websocket state : CloseSent");
                    break;
                case "CloseReceived":
                    Console.WriteLine("Websocket state : CloseReceived");
                    break;
                case "Closed":
                    Console.WriteLine("Websocket state : Closed");
                    await client.ConnectAsync(new Uri(Constants.SocketUrl), cts.Token);
                    ReceiveMessageAsync();
                    break;
                case "Aborted":
                    Console.WriteLine("Websocket state : Aborted");
                    break;
            }

        }
        public async void ReceiveMessageAsync()
        {
            switch (GetSocketState().ToString())
            {
                case "None":
                    Console.WriteLine("ReceiveMessageAsync Websocket state : None");
                    break;
                case "Open":
                    Console.WriteLine("Websocket state : Open , ReceiveMessage");
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
                                Console.WriteLine($"ReceiveAsync: { serialisedMessae}");

                                try
                                {
                                    var msg = JsonConvert.DeserializeObject<Models.Messages>(serialisedMessae);
                                    messages.Add(msg);
                                    listener.RunPlatformCode(messages, msg);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Invalide message format. {ex.Message}");
                                }

                            } while (!result.EndOfMessage);
                        }
                    }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    break;
                case "Connecting":
                    Console.WriteLine("Websocket state : Connecting");
                    break;
                case "CloseSent":
                    Console.WriteLine("Websocket state : CloseSent");
                    break;
                case "CloseReceived":
                    Console.WriteLine("Websocket state : CloseReceived");
                    break;
                case "Closed":
                    Console.WriteLine("Websocket state : Closed");
                    break;
                case "Aborted":
                    Console.WriteLine("Websocket state : Aborted");
                    break;

            }
        }

        public async void SendMessageAsync(string message,string username)
        {
            string web_socket_state = GetSocketState().ToString();
            switch (web_socket_state)
            {
                case "None":
                    Console.WriteLine("Websocket state : None");
                    break;
                case "Open":
                    Console.WriteLine("Websocket state : Open");
                    var segment = CreateMessageObj(message,username);
                    await client.SendAsync(segment, WebSocketMessageType.Text, true, cts.Token);
                    break;
                case "Connecting":
                    Console.WriteLine("Websocket state : Connecting");
                    break;
                case "CloseSent":
                    Console.WriteLine("Websocket state : CloseSent");
                    break;
                case "CloseReceived":
                    Console.WriteLine("Websocket state : CloseReceived");
                    break;
                case "Closed":
                    Console.WriteLine("Websocket state : Closed");
                    break;
                case "Aborted":
                    Console.WriteLine("Websocket state : Aborted");
                    break;

            }
        }

        private ArraySegment<byte> CreateMessageObj(string message , string username)
        {
            var msg = new Models.Messages
            {
                Text = message,
                Type = MessageType.Incoming

                //Name = username,
                //MessagDateTime = DateTime.Now,
                //Text = message,
                //UserId = CrossDeviceInfo.Current.Id

            };

            string serialisedMessage = JsonConvert.SerializeObject(msg);

            var byteMessage = Encoding.UTF8.GetBytes(serialisedMessage);
            var segmnet = new ArraySegment<byte>(byteMessage);
            return segmnet;
        }

        public WebSocketState GetSocketState()
        {
            return client.State;
        }

        public void CloseSocketServerAsync()
        {
            Console.WriteLine("Disposing websocket");

            client.Dispose();
            cts.Dispose();

            //await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Normal Closure", cts.Token);
            //await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal Closure",cts.Token);


        }
    }
}
