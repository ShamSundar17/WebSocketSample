// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace WebSocketsSample.iOS
{
    [Register ("FirstPage")]
    partial class FirstPage
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton startChatButton { get; set; }

        [Action ("StartChatPressed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void StartChatPressed (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (startChatButton != null) {
                startChatButton.Dispose ();
                startChatButton = null;
            }
        }
    }
}