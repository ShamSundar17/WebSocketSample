using Android.App;
using Android.Widget;
using Android.OS;
using SocketSampleShared;
using System.Threading.Tasks;
using System.Threading;
using System.Net.WebSockets;
using SocketSampleShared.Helper;
using SocketSampleShared.Models;
using System.Collections.ObjectModel;
using System;

namespace WebSocketsSample.Droid
{
    [Activity(Label = "SOCKET POC", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity , IListener
    {
        ISocket callsocket;
        public MainActivity currentActivity;
        public static readonly CancellationTokenSource can_tok = new CancellationTokenSource();
        Button button;
        TextView textView;
        EditText editText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            currentActivity = this;

            SetContentView(Resource.Layout.Main);// Set our view from the "main" layout resource

            InitializeViews();

           
        }

        private void InitializeViews()
        {
            button = FindViewById<Button>(Resource.Id.myButton);
            textView = FindViewById<TextView>(Resource.Id.textView1);
            editText = FindViewById<EditText>(Resource.Id.editText1);

            callsocket = new SocketImpl(currentActivity);
            callsocket.ConnectToServerAsync();

            button.Click += delegate 
            {
                if (!string.IsNullOrEmpty(editText.Text) || !string.IsNullOrWhiteSpace(editText.Text))
                {
                    callsocket.SendMessageAsync(editText.Text, "ANDROID");
                }
                else
                {
                    Toast.MakeText(ApplicationContext, "Text is empty", ToastLength.Long).Show();
                }

            };
        }

        public void RunPlatformCode(ObservableCollection<Messages> msgs)
        {
            try
            {
                string displayText = "";
                int i = 0;
                foreach (var data in msgs)
                {
                    if (i > 0)
                    {
                        displayText = displayText + ", " + data.Text;
                    }
                    else
                    {
                        displayText = data.Text;
                    }
                    i++;
                }
                currentActivity.RunOnUiThread(() =>
                {
                    textView.Text = displayText;
                    editText.Text = string.Empty;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("Executing platform code error exce: " + e.Message + e.StackTrace);
            }

        }
    }
}

