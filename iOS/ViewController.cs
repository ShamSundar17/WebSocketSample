using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using SocketSampleShared;
using SocketSampleShared.Helper;
using System.Collections.ObjectModel;
using UIKit;
using SocketSampleShared.Models;
using System.Collections;
using System.Collections.Generic;

namespace WebSocketsSample.iOS
{
    public partial class ViewController : UIViewController //, IListener
    {
        public ISocket socket;
        public IList<SocketSampleShared.Models.Messages> msglist;
        public static readonly CancellationTokenSource can_tok = new CancellationTokenSource();
        //ObservableCollection<SocketSampleShared.Models.Messages> mess;
        public ViewController viewController;

        public ViewController(IntPtr handle) : base(handle)
        {
            viewController = this;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitilizeViews();
        }

        private void InitilizeViews()
        {
            DismissKeyboardOnTouchOutside();
            // Perform any additional setup after loading the view, typically from a nib.
            Button.AccessibilityIdentifier = "myButton";

            //socket = new SocketImpl(viewController);
            //socket.ConnectToServerAsync();

            Button.TouchUpInside += delegate
            {
                if(!string.IsNullOrEmpty(editfield.Text)|| !string.IsNullOrWhiteSpace(editfield.Text))
                {
                    //socket.SendMessageAsync(editfield.Text, "IOS");
                }
                else
                {
                    //Create Alert
                    var okAlertController = UIAlertController.Create("Alert", "Empty field", UIAlertControllerStyle.Alert);

                    //Add Action
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                    // Present Alert
                    PresentViewController(okAlertController, true, null);
                }
            };
            nextButton.TouchUpInside += delegate
            {
                SecondPage secondPage = this.Storyboard.InstantiateViewController("SecondPage") as SecondPage;
                this.NavigationController.PushViewController(secondPage, true);
            };

        }
        private void DismissKeyboardOnTouchOutside()
        {
            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            g.CancelsTouchesInView = false; //for iOS5

            View.AddGestureRecognizer(g);
        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }


        public void RunPlatformCode(ObservableCollection<SocketSampleShared.Models.Messages> msgs)
        {
            msglist = msgs;
            try
            {
                if (msgs.Count > 0)
                {
                    string displayText = "";
                    int i = 0;
                    foreach (var data in msgs)
                    {
                        if (i > 0)
                        {
                            displayText = displayText +", "+ data.Text;
                        }
                        else
                        {
                            displayText = data.Text;
                        }
                        i++;
                    }
                    viewController.BeginInvokeOnMainThread(() =>
                    {
                        textView.Text = displayText;
                        editfield.Text = string.Empty;
                    });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Executing platform code error exce: " + e.Message + e.StackTrace);
            }
        }
    }
}
