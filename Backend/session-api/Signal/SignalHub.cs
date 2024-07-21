using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using session_api.IService;
using session_api.Models;
using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace session_api.Signal
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
            bool exito = int.TryParse(Context.GetHttpContext().Request.Query["userId"], out var userId);

            //TODO: agregar validacion if exito false

            UserSession aUserSession = new UserSession
            {
                userId = userId,
                connectionId = connectionId
            };

            _mySessionService.SetUserSession(userSession: aUserSession);

            Clients.Client(connectionId).SendAsync(ClientMethod.Welcome, aUserSession);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            bool exito = int.TryParse(Context.GetHttpContext().Request.Query["userId"], out var userId);

            //TODO: agregar validacion if exito false
            
            UserSession aUserSession = new UserSession
            {
                userId = userId,
                connectionId = connectionId
            };

            _mySessionService.RemoveUserSession(userSession: aUserSession);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task ReceivePayload(Payload payload)
        {
            _mySessionService.UpdateExistingUsertWithPayload(payload);
            _mySessionService.UpdateExistingUsertWithPayload_TEST(payload);
            await Clients.Client(payload.connectionId).SendAsync("ReceivePayloadResponse", payload);
        }
    }
}
