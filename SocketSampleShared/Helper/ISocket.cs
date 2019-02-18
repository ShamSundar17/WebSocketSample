using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Threading.Tasks;

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
        void RunPlatformCode(IList<Models.Messages> msgs , Models.Messages msgObj);
    }
}
