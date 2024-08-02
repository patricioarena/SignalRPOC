using session_api.IService;
using session_api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Policy;
using session_api.CustomException;

namespace session_api.Service
{
    public class ConnectionUserService : IConnectionUserService
    {
        private ConcurrentDictionary<string, UserUrl> connectionUser = new ConcurrentDictionary<string, UserUrl>()
        {
            //["-eswoeZl3ao8hLANGQwZEQ"] = new UserUrl { userId = 3456, url = "http://localhost:4200/" },
            //["H_KEV01cQrFzJdBN-Fx6lA"] = new UserUrl { userId = 6788, url = "http://localhost:4200/" },
            //["-eswoeZl3ao8hLANGQwZdQ"] = new UserUrl { userId = 3456, url = "http://localhost:4201/" },
            //["H_KEV01cQrFzJdBN-Fx4lA"] = new UserUrl { userId = 6788, url = "http://localhost:4201/" }
        };

        public ConnectionUserService() { }

        public ConcurrentDictionary<string, UserUrl> GetAll() => connectionUser;

        public Task<UserUrl> GetUserUrlByConnectionId(string connectionId)
        {
            return connectionUser.TryGetValue(connectionId, out UserUrl userUrl)
                ? Task.FromResult(userUrl)
                : Task.FromException<UserUrl>(new UserNotFoundException());
        }

        public async Task AddMapConnectionIdUserId(Payload payload)
        {

            await GetUserUrlByConnectionId(payload.connectionId)
                .ContinueWith(async task =>
                {
                    if (task.IsFaulted)
                    {
                        connectionUser.TryAdd(payload.connectionId, new UserUrl { userId = payload.userId, url = payload.url });
                        return Task.CompletedTask;
                    }
                    ///TODO:
                    ///Crear un heartbeat que verifique que los clientes estan conectados periodicamente
                    ///
                    return Task.FromException(new InvalidOperationException());
                });
        }

        public Task RemoveCurrentConnectionFromConnectionUser(string connectionId)
        {
            return RemoveConnectionFromConnectionUser(connectionId)
                ? Task.CompletedTask
                : Task.FromException(new InvalidOperationException());
        }

        public bool RemoveConnectionFromConnectionUser(string connectionId)
        {
            return connectionUser.ContainsKey(connectionId)
              ? connectionUser.TryRemove(connectionId, out _)
              : false;
        }

        /// TODO
        /// Falta la parte de remover el connectionId de la pagina 
        /// para eso:
        ///    usando el connectionId buscamos en ConnectionUserService que url se le corresponde [x]
        ///    ir UrlConnectionService y usar la url para actualizar la lista de sessiones. 
        ///  

    }
}
