using Foundation;
using SocketSampleShared.Helper;
using SocketSampleShared.Models;
using System;
using System.Collections.Generic;
using UIKit;

namespace WebSocketsSample.iOS
{
    public partial class FirstPage : UIViewController, IListener
    {
        public FirstPage (IntPtr handle) : base (handle)
        {
        }

        public void RunPlatformCode(IList<SocketSampleShared.Models.Messages> msgs, SocketSampleShared.Models.Messages msgObj)
        {
            return;
        }

        partial void StartChatPressed(UIButton sender)
        {
            SecondPage secondPage = this.Storyboard.InstantiateViewController("SecondPage") as SecondPage;
            this.NavigationController.PushViewController(secondPage, true);
        }
    }
}