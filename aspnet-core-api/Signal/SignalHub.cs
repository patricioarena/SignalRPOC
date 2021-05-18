using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_core_api.IService;
using aspnet_core_api.Models;
using Microsoft.AspNetCore.SignalR;

namespace aspnet_core_api.Signal
{
    public class SignalHub : Hub
    {
        public IMySessionService _mySessionService { get; set; }
        public SignalHub(IMySessionService mySessionService)
        {
            _mySessionService = mySessionService;
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var connectionName = Context.GetHttpContext().Request.Query["connectionName"];

            UserSession aUserSession = new UserSession
            {
                username = connectionName,
                value = connectionId
            };

            _mySessionService.SetUserSession(userSession: aUserSession);

            Clients.Client(connectionId).SendAsync("WelcomeMethodName", $" {connectionId} => {connectionName}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var connectionName = Context.GetHttpContext().Request.Query["connectionName"];

            UserSession aUserSession = new UserSession
            {
                username = connectionName,
                value = connectionId
            };

            _mySessionService.RemoveUserSession(userSession: aUserSession);
            return base.OnDisconnectedAsync(exception);
        }

        public void GetDataFromClient(string userId, string connectionId)
        {
            Clients.Client(connectionId).SendAsync("clientMethodName", $"Updated userid {userId}");
        }

    }
}
