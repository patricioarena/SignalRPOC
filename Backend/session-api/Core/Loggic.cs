using Microsoft.Extensions.Logging;
using session_api.IService;
using session_api.Model;
using session_api.Service;
using session_api.Signal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.Core
{
    public class Loggic : ILoggic
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IUrlConnectionService _urlConnectionService;
        private readonly IConnectionUserService _connectionUserService;

        public Loggic(ILogger<Loggic> logger,
            IUserService userService, IUrlConnectionService urlConnectionService,
            IConnectionUserService connectionUserService)
        {
            _logger = logger;
            _userService = userService;
            _urlConnectionService = urlConnectionService;
            _connectionUserService = connectionUserService;
        }

        public void SetCurrentConnection(UserConnection userConnection) =>
            _userService.SetCurrentConnection(userConnection: userConnection);

        public async Task SynchronizeRemoveData(UserConnection userConnection)
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

        public async Task SynchronizeUpdateData(Payload payload)
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

        public void LogTaskError(string methodName, Task task)
        {
            if (task.IsFaulted)
                _logger.LogError(methodName, task.Exception?.GetBaseException().Message);
        }

        public List<User> GetUsersForUrl()
        {
            _urlConnectionService.GetListConnectionsByUrl("");
            return MockGetListUser();
        }

        private List<User> MockGetListUser() => new() {
            new User
            {
                userId = 1,
                username = "CyberPhoenix",
                picture = "https://mighty.tools/mockmind-api/content/human/65.jpg",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY3g", "DT89mQ4bFYHq7PpUOEfY9h" }
            },
            new User
            {
                userId = 2,
                username = "PixelWarrior",
                picture = "https://mighty.tools/mockmind-api/content/human/45.jpg",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY9i" }
            },
            new User
            {
                userId = 3,
                username = "QuantumRider",
                picture = "https://mighty.tools/mockmind-api/content/human/49.jpg",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9j", "DT19mQ4bFYHq7PpUOEfY9k" }
            },
            new User
            {
                userId = 4,
                username = "NeonSpecter",
                picture = "https://mighty.tools/mockmind-api/content/human/4.jpg",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 5,
                username = "DarkPhoenix",
                picture = "https://mighty.tools/mockmind-api/content/cartoon/7.jpg",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY3h", "DT89mQ4bFYHq7PpUOEfY9i" }
            },
            new User
            {
                userId = 6,
                username = "PixelPerfect",
                picture = "https://mighty.tools/mockmind-api/content/cartoon/10.jpg",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY9j" }
            },
            new User
            {
                userId = 7,
                username = "GenericPerson",
                picture = "https://mighty.tools/mockmind-api/content/human/42.jpg",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9k", "DT19mQ4bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 8,
                username = "NeoFox",
                picture = "https://mighty.tools/mockmind-api/content/human/55.jpg",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 9,
                username = "Rick",
                picture = "https://mighty.tools/mockmind-api/content/cartoon/11.jpg",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9k", "DT19mQ4bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 10,
                username = "Morty",
                picture = "https://mighty.tools/mockmind-api/content/human/56.jpg",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            }
        };
    }
}

