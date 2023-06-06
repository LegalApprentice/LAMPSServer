using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics;
using LAMPSServer.Models;

// https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-5.0&tabs=visual-studio

//dotnet dev-certs https --clean
//dotnet dev-certs https --trust


// https://www.youtube.com/watch?v=Lws0zOaseIM

namespace LAMPSServer.Hubs
{
    public class DocumentHub : Hub
    {
        public static int userCount = 0;
        // https://docs.microsoft.com/en-us/aspnet/signalr/overview/advanced/dependency-injection

        //private readonly PresenceTracker presenceTracker;


        public DocumentHub()
        {
        }


        public override async Task OnConnectedAsync()
        {
            DocumentHub.userCount += 1;

 
            await base.OnConnectedAsync();
        }

         public override async Task OnDisconnectedAsync(Exception exception)
        {
            DocumentHub.userCount -= 1;

            await base.OnDisconnectedAsync(exception);
        }



        public async Task TopicPayload(string topic, object payload)
        {
            if ( DocumentHub.userCount == 1 ) {
                await Clients.Caller.SendAsync("TopicPayload", "warning", "Open Legal Pad");
            } else {

                await Clients.Others.SendAsync("TopicPayload", topic, payload);
                var caller = $"{topic} shared";
                await Clients.Caller.SendAsync("TopicPayload", "info", caller);
            }
        }

        public async Task Plugin(string topic, object payload)
        {
            await Clients.Others.SendAsync("Plugin", topic, payload);
        }

        public async Task Start(object payload)
        {
            await Clients.All.SendAsync("Start", payload);
        }
        public async Task Stop(object payload)
        {
            await Clients.All.SendAsync("Stop", payload);
        }
        public async Task Ping(string payload)
        {
            var others = $"Sent to Others {payload}";
            await Clients.Others.SendAsync("Pong", others);
            var caller = $"Sent to Caller {payload}";
            await Clients.Caller.SendAsync("Pong", caller);
        }
    }
}