using System;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public void SendToAll(SignalRMessage message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(message.Name, message.Message);
        }
    }
}