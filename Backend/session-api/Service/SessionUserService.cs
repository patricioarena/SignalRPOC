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
    public class SessionUserService : ISessionUserService
    {
        private ConcurrentDictionary<string, UserUrl> sessionUser = new ConcurrentDictionary<string, UserUrl>()
        {
            ["-eswoeZl3ao8hLANGQwZEQ"] = new UserUrl { userId = 3456, url = "http://localhost:4200/" },
            ["H_KEV01cQrFzJdBN-Fx6lA"] = new UserUrl { userId = 6788, url = "http://localhost:4200/" },
            ["-eswoeZl3ao8hLANGQwZdQ"] = new UserUrl { userId = 3456, url = "http://localhost:4201/" },
            ["H_KEV01cQrFzJdBN-Fx4lA"] = new UserUrl { userId = 6788, url = "http://localhost:4201/" }
        };

        public SessionUserService() { }

        public ConcurrentDictionary<string, UserUrl> GetSessionsOfUsers() 
        {
            return sessionUser;
        }

        public UserUrl GetUserIdByConnectionId(string connectionId)
        {
            return sessionUser.TryGetValue(connectionId, out UserUrl userUrl) ? userUrl : null;
        }

        public Task AddMapConnectionIdUserId(Payload payload)
        {
            try
            {
                var existingSessionUser = GetUserIdByConnectionId(payload.connectionId);
                if (existingSessionUser == null)
                {
                    sessionUser[payload.connectionId] = new UserUrl { userId = payload.userId, url = payload.url };
                    return Task.CompletedTask; // Representa éxito sin valor de retorno
                }
                return Task.FromException(new NotAddedMapping());
            }
            catch (Exception ex)
            {
                return Task.FromException(ex); // Representa error en caso de excepción
            }
        }

        /// TODO
        /// Falta la parte de remover el connectionId de la pagina 
        /// para eso usando el connectionId buscamos en SessionUserService que url se le corresponde 
        /// y extraemos la url con esa vamos al UrlSessionService y usando la url actualizamos la lista de sessiones. 
    }
}
