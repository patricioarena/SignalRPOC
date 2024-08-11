using Microsoft.AspNetCore.SignalR;
using session_api.Core;
using session_api.Model;
using System;
using System.Threading.Tasks;

namespace session_api.Signal
{
    public class SignalHub : Hub
    {
        private readonly ILoggic _loggic;
        private readonly string HEADER_USER_ID = "UserId";

        public SignalHub(ILoggic loggic)
        {
            _loggic = loggic;
        }

        public override Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            string auxUserId = Context.GetHttpContext().Request.Query[HEADER_USER_ID];
            bool success = int.TryParse(auxUserId, out var userId);

            //TODO: agregar validacion if success false

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
            string connectionId = Context.ConnectionId;
            string auxUserId = Context.GetHttpContext().Request.Query[HEADER_USER_ID];
            bool success = int.TryParse(auxUserId, out var userId);

            //TODO: agregar validacion if success false

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
            ///TODO: si algunos de los datos del payload esta null 
            /// ver como lo manejamos.

            await _loggic.SynchronizeUpdateData(payload);
            await Clients.Client(payload.connectionId).SendAsync(ClientMethod.Received_Data, payload);

            //var list = _loggic.GetUsersForUrl(payload.url);
            //await Clients.Client(payload.connectionId).SendAsync(ClientMethod.Received_Data, list);
        }

        ///TODO: Crear un heartbeat que verifique que los clientes estan conectados periodicamente

        ///TODO: La data que se envia hay que wrappearla en el Result.Response
    }
}
