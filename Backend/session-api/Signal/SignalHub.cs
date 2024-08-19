using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using session_api.Contant;
using session_api.Core;
using session_api.Model;
using session_api.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.Signal
{
    public class SignalHub : Hub
    {
        private readonly ILoggic _loggic;
        private readonly IConfiguration _configuration;
        private readonly IValidator<Payload> _payloadValidator;
        private readonly string HEADER_USER_ID = "UserId";

        public SignalHub(ILoggic loggic, IValidator<Payload> payloadValidator, IConfiguration configuration)
        {
            _loggic = loggic;
            _payloadValidator = payloadValidator;
            _configuration = configuration;
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

            Clients.Client(connectionId)
                .SendAsync(ClientMethod.Welcome,
                    Response<UserConnection, object>.Builder()
                    .SetData(aUserConnection)
                    .Build());
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
                    _loggic.LogTaskFaulted(nameof(_loggic.SynchronizeRemoveData), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await base.OnDisconnectedAsync(exception);
                })
                .Unwrap();
        }

        public async Task NotifyConnection(Payload payload)
        {
            var validationResult = _payloadValidator.Validate(payload);

            if (!validationResult.IsValid)
            {
                await Clients.Caller.SendAsync(ClientMethod.Validation_Error, validationResult.Errors);
                return;
            }

            await _loggic.SynchronizeUpdateData(payload)
                .ContinueWith(async task =>
                {
                    if (!task.IsFaulted && _configuration.GetValue<bool>("ReturnTheUserCreatedFromPayload"))
                    {
                        var data = _loggic.GetUserByUserId(payload.userId).Result;
                        var metadata = new Dictionary<string, object>
                        {
                            [Metadata.TypeData] = Metadata.Object,
                        };

                        await Clients.Client(payload.connectionId)
                            .SendAsync(ClientMethod.Received_Data,
                                 Response<User, object>.Builder()
                                 .SetData(data)
                                 .SetMetadata(metadata)
                                 .Build());
                    }
                })
                .Unwrap();

            await _loggic.GetConnectionUserWithFiterAsync(payload.pageUrl, payload.userId)
                .ContinueWith(async task =>
                {
                    if (!task.IsFaulted)
                    {
                        var data = task.Result;
                        var metadata = new Dictionary<string, object>
                        {
                            [Metadata.TypeData] = Metadata.Array,
                        };

                        await Clients.Client(payload.connectionId)
                            .SendAsync(ClientMethod.Received_Data,
                                Response<List<User>, object>.Builder()
                                .SetData(data)
                                .SetMetadata(metadata)
                                .Build());
                    }
                })
                .Unwrap();
        }

        ///TODO: Crear un heartbeat que verifique que los clientes estan conectados periodicamente

        ///TODO: La data que se envia hay que wrappearla en el Result.Response
    }
}
