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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace session_api.Service
{
    public class UrlConnectionService : IUrlConnectionService
    {
        private ConcurrentDictionary<string, List<string>> urlListConnections = new ConcurrentDictionary<string, List<string>>()
        {
            //["http://localhost:4200/"] = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "H_KEV01cQrFzJdBN-Fx6lA" },
            //["http://localhost:4201/"] = new List<string> { "-eswoeZl3ao8hLANGQwZdQ", "H_KEV01cXrFzJdBN-Fx4lA" }
        };

        public UrlConnectionService() { }

        public ConcurrentDictionary<string, List<string>> GetAll() => urlListConnections;

        public List<string> GetListConnectionsByUrl(string url)
        {
            return urlListConnections.TryGetValue(url, out List<string> listConnection) ? listConnection : null;
        }

        // TODO: Se puede separar en dos add y update, creo
        public Task AddConnectionToListConnectionsIfNotExist(Payload payload)
        {
            try
            {
                var existingList = GetListConnectionsByUrl(payload.url);
                if (existingList == null)
                {
                    urlListConnections.TryAdd(payload.url, new List<string> { payload.connectionId });
                    return Task.CompletedTask; // Representa éxito sin valor de retorno
                }

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

        public Task RemoveCurrentConnectionFromUrl(string connectionId, string url)
        {
            return RemoveConnectionFromUrl(connectionId, url)
                ? Task.CompletedTask
                : Task.FromException(new InvalidOperationException());
        }

        private bool RemoveConnectionFromUrl(string connectionId, string url)
        {
            var urlList = urlListConnections[url];
            var isSuccessRemoved = urlList.Remove(connectionId);

            if(isSuccessRemoved && urlList.Count > 0)
                return true;

            return urlList.Count == 0
                ? urlListConnections.TryRemove(url, out _)
                : false;
        }
    }
}
