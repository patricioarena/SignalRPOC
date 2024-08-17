using Microsoft.Extensions.Logging;
using session_api.IService;
using session_api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    await _urlConnectionService.RemoveCurrentConnectionFromUrl(userConnection.connectionId, task.Result.Url);
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
                    await _userService.RemoveCurrentConnectionFromUserAsync(userConnection: userConnection);
                })
                .Unwrap()
                .ContinueWith(task => LogTaskError(nameof(_userService.RemoveCurrentConnectionFromUserAsync), task));
        }

        public async Task SynchronizeUpdateData(Payload payload)
        {
            await _userService.UpdateUser(payload)
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_userService.UpdateUser), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _urlConnectionService.SetCurrentConnectionToUrlAsync(payload);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_urlConnectionService.SetCurrentConnectionToUrlAsync), task);

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

        public async Task<List<User>> GetUsersForUrl(string url)
        {
            List<User> result = new List<User>();
            await _urlConnectionService.GetListConnectionsByUrlAsync(url)
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_urlConnectionService.GetListConnectionsByUrlAsync), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    return await extractUniqueUserUrls(task);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskError(nameof(_connectionUserService.GetUserUrlByConnectionId), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    return await extractUserOfUniqueUserUrls(task);
                })
                .Unwrap()
                .ContinueWith(task =>
                {
                    LogTaskError(nameof(_userService.GetUserByUserIdAsync), task);

                    if (!task.IsFaulted)
                    {
                        result = task.Result
                            .Concat(RandomMockEngine())
                            .ToList();
                    }
                });

            return result;
        }

        private async Task<List<User>> extractUserOfUniqueUserUrls(Task<HashSet<UserUrl>> task)
        {
            var uniqueUserUrls = task.Result;
            var tasksUsers = uniqueUserUrls.Select(userUrl => _userService.GetUserByUserIdAsync(userUrl.UserId));
            var users = await Task.WhenAll(tasksUsers);

            return users.ToList();
        }

        private async Task<HashSet<UserUrl>> extractUniqueUserUrls(Task<List<string>> task)
        {
            var connections = task.Result;
            var tasksUserUrls = connections.Select(_connectionUserService.GetUserUrlByConnectionId);
            var userUrls = await Task.WhenAll(tasksUserUrls);
            var uniqueUserUrls = new HashSet<UserUrl>(userUrls);

            return uniqueUserUrls;
        }

        private List<User> RandomMockEngine()
        {
            Random random = new Random();
            int numberOfRandomElements = random.Next(2, 6);

            return MockGetListUser()
                .OrderBy(x => random.Next())
                .Take(numberOfRandomElements)
                .ToList();
        }

        private List<User> MockGetListUser() => new()
        {
            new User
            {
                userId = 1341,
                username = "CyberPhoenix",
                picture = "https://mighty.tools/mockmind-api/content/human/65.jpg",
                mail = "cyberphoenix@example.com",
                fullname = "Phoenix Blaze",
                position = "Software Engineer",
                Role = "Admin",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY3g", "DT89mQ4bFYHq7PpUOEfY9h" }
            },
            new User
            {
                userId = 1552,
                username = "PixelWarrior",
                picture = "https://mighty.tools/mockmind-api/content/human/45.jpg",
                mail = "pixelwarrior@example.com",
                fullname = "Warren Pixels",
                position = "Graphic Designer",
                Role = "User",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY9i" }
            },
            new User
            {
                userId = 1233,
                username = "QuantumRider",
                picture = "https://mighty.tools/mockmind-api/content/human/49.jpg",
                mail = "quantumrider@example.com",
                fullname = "Quinn Rider",
                position = "Data Scientist",
                Role = "Moderator",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9j", "DT19mQ4bFYHq7PpUOEfY9k" }
            },
            new User
            {
                userId = 1864,
                username = "NeonSpecter",
                picture = "https://mighty.tools/mockmind-api/content/human/4.jpg",
                mail = "neonspecter@example.com",
                fullname = "Samantha Specter",
                position = "UI/UX Designer",
                Role = "User",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 1275,
                username = "DarkPhoenix",
                picture = "https://mighty.tools/mockmind-api/content/cartoon/7.jpg",
                mail = "darkphoenix@example.com",
                fullname = "Damian Phoenix",
                position = "DevOps Engineer",
                Role = "Admin",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY3h", "DT89mQ4bFYHq7PpUOEfY9i" }
            },
            new User
            {
                userId = 1176,
                username = "PixelPerfect",
                picture = "https://mighty.tools/mockmind-api/content/cartoon/10.jpg",
                mail = "pixelperfect@example.com",
                fullname = "Patricia Pixels",
                position = "Photographer",
                Role = "User",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY9j" }
            },
            new User
            {
                userId = 1447,
                username = "GenericPerson",
                picture = "https://mighty.tools/mockmind-api/content/human/42.jpg",
                mail = "genericperson@example.com",
                fullname = "George Person",
                position = "Content Writer",
                Role = "User",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9k", "DT19mQ4bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 1208,
                username = "NeoFox",
                picture = "https://mighty.tools/mockmind-api/content/human/55.jpg",
                mail = "neofox@example.com",
                fullname = "Natalie Fox",
                position = "Frontend Developer",
                Role = "User",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 1489,
                username = "Rick",
                picture = "https://mighty.tools/mockmind-api/content/cartoon/11.jpg",
                mail = "rick@example.com",
                fullname = "Rick Sanchez",
                position = "Scientist",
                Role = "Admin",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9k", "DT19mQ4bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 1710,
                username = "Morty",
                picture = "https://mighty.tools/mockmind-api/content/human/56.jpg",
                mail = "morty@example.com",
                fullname = "Morty Smith",
                position = "Student",
                Role = "User",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            }
        };

    }
}

