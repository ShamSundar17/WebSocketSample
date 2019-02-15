using System;
using System.Collections.Generic;
using UIKit;

namespace WebSocketsSample.iOS
{
    public partial class ChatViewController : UIViewController
    {
        public ChatViewController() : base("ChatViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

