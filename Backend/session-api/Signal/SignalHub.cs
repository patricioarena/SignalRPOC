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
        public IUserService _userService { get; set; }
        public IUrlSessionService _urlSessionService { get; set; }
        public ISessionUserService _sessionUserService { get; set; }

        public SignalHub(IUserService mySessionService, IUrlSessionService urlSessionService, ISessionUserService sessionUserService)
        {
            _userService = mySessionService;
            _urlSessionService = urlSessionService;
            _sessionUserService = sessionUserService;
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

            _userService.SetUserSession(userSession: aUserSession);

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

            _userService.RemoveUserSession(userSession: aUserSession);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task NotifyConnection(Payload payload)
        {
            await SynchronizeData(payload);
            await Clients.Client(payload.connectionId).SendAsync("ReceivePayloadResponse", payload);
        }

        private async Task SynchronizeData(Payload payload)
        {
            await _userService.UpdateUserIfEmptyFields(payload)
                .ContinueWith(async task =>
                {
                    LogTaskError(task);
                    if (task.IsFaulted)
                        await Task.FromException(task.Exception); // Propaga el error
                    await _urlSessionService.AddConnectionToUserSessionIfNotExist(payload);
                })
                .Unwrap() // Desenrolla el Task<Task> devuelto por ContinueWith
                .ContinueWith(async task =>
                {
                    LogTaskError(task);
                    if (task.IsFaulted)
                        await Task.FromException(task.Exception); // Propaga el error
                    await _sessionUserService.AddMapConnectionIdUserId(payload);
                })
                .Unwrap() // Desenrolla el Task<Task> devuelto por ContinueWith
                .ContinueWith(LogTaskError); // Maneja cualquier error final
        }

        private static void LogTaskError(Task task)
        {
            if (task.IsFaulted)
            {
                Console.WriteLine($"Se produjo un error: {task.Exception?.GetBaseException().Message}");
            }
        }
    }
}
