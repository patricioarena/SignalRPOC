using System;
using System.Threading.Tasks;
using session_api.Model;
using session_api.Core;
using Microsoft.AspNetCore.SignalR;

namespace session_api.Signal
{
    public class SignalHub : Hub
    {
        private readonly ILoggic _loggic;

        public SignalHub(ILoggic loggic)
        {
            _loggic = loggic;
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

            _loggic.SetCurrentConnection(userConnection: aUserConnection);

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

            await _loggic.SynchronizeRemoveData(aUserConnection)
                .ContinueWith(async task =>
                {
                    _loggic.LogTaskError(nameof(_loggic.SynchronizeRemoveData), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await base.OnDisconnectedAsync(exception);
                })
                .Unwrap();
        }

        public async Task NotifyConnection(Payload payload)
        {
            await _loggic.SynchronizeUpdateData(payload);
            await Clients.Client(payload.connectionId).SendAsync(ClientMethod.Received_Data, payload);

            var list = _loggic.GetUsersForUrl();
            await Clients.Client(payload.connectionId).SendAsync(ClientMethod.Received_Data, list);
        }

        ///TODO: Crear un heartbeat que verifique que los clientes estan conectados periodicamente

        ///TODO: El mismo usuario puede tener la misma pagina en diferentes pestañas, navegadores, etc.
        /// Para la lista de usuarios que se debe retornar tomar en cuenta que no pueden haber usuarios
        /// repetidos para una misma pagina.

        ///TODO: La data que se envia hay que wrappearla en el Result.Response
    }
}
