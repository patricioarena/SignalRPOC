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
    public class UrlSessionService : IUrlSessionService
    {
        private ConcurrentDictionary<string, List<string>> urlListSession = new ConcurrentDictionary<string, List<string>>()
        {
            ["http://localhost:4200/"] = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "H_KEV01cQrFzJdBN-Fx6lA" },
            ["http://localhost:4201/"] = new List<string> { "-eswoeZl3ao8hLANGQwZdQ", "H_KEV01cXrFzJdBN-Fx4lA" }
        };

        public UrlSessionService() { }

        public List<string> GetListSession(string url)
        {
            return urlListSession.TryGetValue(url, out List<string> listSession) ? listSession : null;
        }

        public ConcurrentDictionary<string, List<string>> GetUrlListSession()
        {
            return urlListSession;
        }

        public Task AddConnectionToUserSessionIfNotExist(Payload payload)
        {
            try
            {
                var existingList = GetListSession(payload.url);
                if (existingList != null)
                {
                    if (!urlListSession[payload.url].Contains(payload.connectionId))
                    {
                        urlListSession[payload.url].Add(payload.connectionId);
                    }
                    return Task.CompletedTask; // Representa éxito sin valor de retorno
                }
                else
                {
                    return Task.FromException(new NotAddedConnectionOnUrl());
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex); // Representa error en caso de excepción
            }
        }
    }
}
