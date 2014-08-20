using System;
using Microsoft.AspNet.SignalR.Client;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;

namespace Android
{
	[Activity (Label = "Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;
		private HubConnection _hubConnection;

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			var textView = FindViewById<TextView> (Resource.Id.textView1);

			// Connect to the server
			_hubConnection = new HubConnection ("http://xamarinsignalr.azurewebsites.net/");

			// Create a proxy to the 'ChatHub' SignalR Hub
			var chatHubProxy = _hubConnection.CreateHubProxy ("ChatHub");
      
			// Wire up a handler for the 'UpdateChatMessage' for the server
			// to be called on our client
			chatHubProxy.On<string,string> ("broadcastMessage", (name, message) => {
				RunOnUiThread (() => {
					textView.Text = string.Format ("Received Msg: {0}\r\n", message);
				});
			}
			);

			try {
				await _hubConnection.Start ();
			} catch (Exception e) {
				throw;
			}
			// Start the connection

			var messageObj = new SignalRMessage{ Name = "Android", Message = "Hello" };
			// Invoke the 'UpdateNick' method on the server
			await chatHubProxy.Invoke ("Send", messageObj);
		}


		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			_hubConnection.Dispose ();

		}
	}
}

