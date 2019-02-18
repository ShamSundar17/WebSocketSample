using System;
using Plugin.DeviceInfo;

namespace SocketSampleShared.Models
{
    public class Messages
    {
        //public string Text { get; set; }
        //public DateTime MessagDateTime { get; set; }

        //public bool IsIncoming => UserId != CrossDeviceInfo.Current.Id;

        //public string Name { get; set; }
        //public string UserId { get; set; }

        public MessageType Type { get; set; }
        public string Text { get; set; }
    }

    public enum MessageType
    {
        Incoming,
        Outgoing,
    }
}
