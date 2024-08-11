﻿using session_api.IService;
using session_api.Model;
using session_api.Result;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace session_api.Service
{
    public class ConnectionUserService : IConnectionUserService
    {
        private ConcurrentDictionary<string, UserUrl> connectionUser = new ConcurrentDictionary<string, UserUrl>()
        {
            ["-eswoeZl3ao8hLANGQwZEQ"] = new UserUrl(3456, "http://localhost:4200/"),
            ["H_KEV01cQrFzJdBN-Fx6lA"] = new UserUrl(6788, "http://localhost:4200/"),
            ["-eswoeZl3ao8hLANGQwZdQ"] = new UserUrl(3456, "http://localhost:4201/"),
            ["H_KEV01cQrFzJdBN-Fx4lA"] = new UserUrl(6788, "http://localhost:4201/")
        };

        public ConnectionUserService() { }

        public ConcurrentDictionary<string, UserUrl> GetAllConnectionUserMappings() => connectionUser;

        public Task<UserUrl> GetUserUrlByConnectionId(string connectionId)
        {
            return connectionUser.TryGetValue(connectionId, out UserUrl userUrl)
                ? Task.FromResult(userUrl)
                : Task.FromException<UserUrl>(new CustomException(CustomException.ErrorsEnum.UserNotFoundException));
        }

        public async Task AddMapConnectionIdUserId(Payload payload)
        {

            await GetUserUrlByConnectionId(payload.connectionId)
                .ContinueWith(async task =>
                {
                    if (task.IsFaulted)
                    {
                        connectionUser.TryAdd(payload.connectionId, new UserUrl(payload.userId, payload.url));
                        return Task.CompletedTask;
                    }
                    ///TODO: Crear un heartbeat que verifique que los clientes estan conectados periodicamente
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
    }
}
