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
    public class UrlConnectionService : IUrlConnectionService
    {
        private ConcurrentDictionary<string, List<string>> urlListConnections = new ConcurrentDictionary<string, List<string>>()
        {
            ["http://localhost:4200/"] = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "H_KEV01cQrFzJdBN-Fx6lA" },
            ["http://localhost:4201/"] = new List<string> { "-eswoeZl3ao8hLANGQwZdQ", "H_KEV01cXrFzJdBN-Fx4lA" }
        };

        public UrlConnectionService() { }

        public ConcurrentDictionary<string, List<string>> GetAll() => urlListConnections;

        public List<string> GetListConnectionsByUrl(string url)
        {
            return urlListConnections.TryGetValue(url, out List<string> listConnection) ? listConnection : null;
        }

        public Task AddConnectionToListConnectionsIfNotExist(Payload payload)
        {
            try
            {
                var existingList = GetListConnectionsByUrl(payload.url);
                if (existingList != null)
                {
                    if (!urlListConnections[payload.url].Contains(payload.connectionId))
                    {
                        urlListConnections[payload.url].Add(payload.connectionId);
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
