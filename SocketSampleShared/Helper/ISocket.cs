using System;
using System.Collections.ObjectModel;
using System.Net.WebSockets;

namespace SocketSampleShared.Helper
{
    public interface ISocket
    {
        void ConnectToServerAsync();
        void ReceiveMessageAsync();
        void SendMessageAsync(string msg,string username);
        WebSocketState GetSocketState();
        void CloseSocketServerAsync();
    }

    public interface IListener 
    {
        void RunPlatformCode(ObservableCollection<Models.Messages> msgs);
    }
}
