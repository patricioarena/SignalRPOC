using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using session_api.IService;
using session_api.Models;
using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;
using session_api.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Logging;

namespace session_api.Signal
{
    public class SignalHub : Hub
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IUrlConnectionService _urlConnectionService;
        private readonly IConnectionUserService _connectionUserService;

        public SignalHub(ILogger<SignalHub> logger, IUserService userService, IUrlConnectionService urlConnectionService, IConnectionUserService connectionUserService)
        {
            _logger = logger;
            _userService = userService;
            _urlConnectionService = urlConnectionService;
            _connectionUserService = connectionUserService;
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            bool successful = int.TryParse(Context.GetHttpContext().Request.Query["userId"], out var userId);

            //TODO: agregar validacion if successful false

            UserConnection aUserConnection = new UserConnection
            {
                userId = userId,
                connectionId = connectionId
            };

            _userService.SetCurrentConnection(userConnection: aUserConnection);

            Clients.Client(connectionId).SendAsync(ClientMethod.Welcome, aUserConnection);
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            bool successful = int.TryParse(Context.GetHttpContext().Request.Query["userId"], out var userId);

            //TODO: agregar validacion if successful false

            UserConnection aUserConnection = new UserConnection
            {
                userId = userId,
                connectionId = connectionId
            };

            await SynchronizeRemoveData(aUserConnection)
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(SynchronizeRemoveData), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await base.OnDisconnectedAsync(exception);
                })
                .Unwrap();
        }

        public async Task NotifyConnection(Payload payload)
        {
            await SynchronizeUpdateData(payload);
            await Clients.Client(payload.connectionId).SendAsync("ReceivePayloadResponse", payload);
        }
        private async Task SynchronizeRemoveData(UserConnection userConnection)
        {
            await _connectionUserService.GetUserUrlByConnectionId(userConnection.connectionId)
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_connectionUserService.GetUserUrlByConnectionId), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _urlConnectionService.RemoveCurrentConnectionFromUrl(userConnection.connectionId, task.Result.url);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_urlConnectionService.RemoveCurrentConnectionFromUrl), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _connectionUserService.RemoveCurrentConnectionFromConnectionUser(userConnection.connectionId);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_connectionUserService.RemoveCurrentConnectionFromConnectionUser), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _userService.RemoveCurrentConnectionFromUser(userConnection: userConnection);
                })
                .Unwrap()
                .ContinueWith(task => LogTaskError(nameof(_userService.RemoveCurrentConnectionFromUser), task)); 
        }

        private async Task SynchronizeUpdateData(Payload payload)
        {
            await _userService.UpdateUserIfEmptyFields(payload)
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_userService.UpdateUserIfEmptyFields), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _urlConnectionService.AddConnectionToListConnectionsIfNotExist(payload);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_urlConnectionService.AddConnectionToListConnectionsIfNotExist), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _connectionUserService.AddMapConnectionIdUserId(payload);
                })
                .Unwrap()
                .ContinueWith(task => LogTaskError(nameof(_connectionUserService.AddMapConnectionIdUserId), task));
        }

        private void LogTaskError(string methodName, Task task)
        {
            if (task.IsFaulted)
                _logger.LogError(methodName, task.Exception?.GetBaseException().Message);
        }

        ///TODO:
        ///Crear un heartbeat que verifique que los clientes estan conectados periodicamente
    }
}
