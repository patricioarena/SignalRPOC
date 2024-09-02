using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using connected_hub_api.IService;
using connected_hub_api.Model;
using connected_hub_api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace connected_hub_api.Core
{
    public class Loggic : ILoggic
    {
        private readonly ILogger _logger;

        private readonly IUserService _userService;

        private readonly IUrlConnectionService _urlConnectionService;

        private readonly IConnectionUserService _connectionUserService;

        private readonly IConfiguration _configuration;

        public Loggic(ILogger<Loggic> logger,
            IUserService userService, IUrlConnectionService urlConnectionService,
            IConnectionUserService connectionUserService,
            IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _urlConnectionService = urlConnectionService;
            _connectionUserService = connectionUserService;
            _configuration = configuration;
        }
        public async Task<User> GetUserByUserId(int userId) => await _userService.GetUserByUserIdAsync(userId);

        public void SetCurrentConnection(UserConnection userConnection) =>
            _userService.SetCurrentConnection(userConnection: userConnection);

        public async Task SynchronizeRemoveData(UserConnection userConnection)
        {
            await _connectionUserService.GetUserUrlByConnectionId(userConnection.connectionId)
                .ContinueWith(async task =>
                {
                    LogTaskFaulted(nameof(_connectionUserService.GetUserUrlByConnectionId), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _urlConnectionService.RemoveCurrentConnectionFromUrl(userConnection.connectionId, task.Result.Url);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskFaulted(nameof(_urlConnectionService.RemoveCurrentConnectionFromUrl), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _connectionUserService.RemoveCurrentConnectionFromConnectionUser(userConnection.connectionId);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskFaulted(nameof(_connectionUserService.RemoveCurrentConnectionFromConnectionUser), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _userService.RemoveCurrentConnectionFromUserAsync(userConnection: userConnection);
                })
                .Unwrap()
                .ContinueWith(task => LogTaskFaulted(nameof(_userService.RemoveCurrentConnectionFromUserAsync), task));
        }

        public async Task SynchronizeUpdateData(Payload payload)
        {
            await _userService.UpdateUser(payload)
                .ContinueWith(async task =>
                {
                    LogTaskFaulted(nameof(_userService.UpdateUser), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _urlConnectionService.SetCurrentConnectionToUrlAsync(payload);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskFaulted(nameof(_urlConnectionService.SetCurrentConnectionToUrlAsync), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    await _connectionUserService.AddMapConnectionIdUserId(payload);
                })
                .Unwrap()
                .ContinueWith(task => LogTaskFaulted(nameof(_connectionUserService.AddMapConnectionIdUserId), task));
        }

        public void LogTaskFaulted(string methodName, Task task)
        {
            if (task.IsFaulted)
                _logger.LogDebug(methodName, task.Exception?.GetBaseException().Message);
        }

        public async Task<List<User>> GetUsersForUrl(string url)
        {
            List<User> result = new List<User>();
            await _urlConnectionService.GetListConnectionsByUrlAsync(url)
                .ContinueWith(async task =>
                {
                    LogTaskFaulted(nameof(_urlConnectionService.GetListConnectionsByUrlAsync), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    return await extractUniqueUserUrls(task);
                })
                .Unwrap()
                .ContinueWith(async task =>
                {
                    LogTaskFaulted(nameof(extractUniqueUserUrls), task);

                    if (task.IsFaulted)
                        await Task.FromException(task.Exception);
                    return await extractUserOfUniqueUserUrls(task);
                })
                .Unwrap()
                .ContinueWith(task =>
                {
                    LogTaskFaulted(nameof(extractUserOfUniqueUserUrls), task);

                    if (task.IsFaulted)
                    {
                        if (_configuration.GetValue<bool>("Mock:ExtraResult"))
                            result = result.Concat(RandomMockEngine()).ToList();
                    }

                    if (!task.IsFaulted)
                    {
                        result = _configuration.GetValue<bool>("Mock:ExtraResult")
                            ? task.Result.Concat(RandomMockEngine()).ToList()
                            : task.Result.ToList();
                    }
                });

            return result;
        }

        public async Task<List<User>> GetConnectionUserWithFiterAsync(string base64URL, int? exclude)
        {
            var decodedUrl = Decode.Base64Url(base64URL);
            var users = GetUsersForUrl(decodedUrl).Result;

            // Aplicar filtro si es necesario
            return (exclude.HasValue)
                ? await Task.FromResult(users.Where(user => user.userId != exclude.Value).ToList())
                : await Task.FromResult(users);
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
            int numberOfRandomElements = random.Next(5, 9);

            return MockGetListUser()
                .OrderBy(x => random.Next())
                .Take(numberOfRandomElements)
                .ToList();
        }

        private List<User> MockGetListUser() => new()
        {
            new User
            {
                userId = 2,
                username = "FISCALIA\\nahuel",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/2",
                mail = "nahuel@fepba.gov.ar",
                fullname = "Milanesi, Gabriel Nahuel",
                position = "Director",
                role = "Admin",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY3g", "DT89mQ4bFYHq7PpUOEfY9h" }
            },
            new User
            {
                userId = 4,
                username = "FISCALIA\\pcasco",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/4",
                mail = "pcasco@fepba.gov.ar",
                fullname = "Gonzalez Casco, Pablo Andrés",
                position = "Ingeniero",
                role = "User",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY9i" }
            },
            new User
            {
                userId = 5,
                username = "FISCALIA\\csolan",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/5",
                mail = "cristina@fepba.gov.ar",
                fullname = "Solan, Cristina Isabel",
                position = "Analista Funcional",
                role = "Moderator",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9j", "DT19mQ4bFYHq7PpUOEfY9k" }
            },
            new User
            {
                userId = 10596,
                username = "FISCALIA\\parena",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/10596",
                mail = "parena@fepba.gov.ar",
                fullname = "Arena, Patricio Ernesto Antonio",
                position = "Desarrollador",
                role = "User",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 10627,
                username = "FISCALIA\\mlambolla",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/10627",
                mail = "mlambolla@fepba.gov.ar",
                fullname = "Lambolla, Mariano",
                position = "Jefe de Departamento de Programación",
                role = "Admin",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY3h", "DT89mQ4bFYHq7PpUOEfY9i" }
            },
            new User
            {
                userId = 10653,
                username = "FISCALIA\\fmeza",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/10653",
                mail = "fmeza@fepba.gov.ar",
                fullname = "Meza, Fernando Martín",
                position = "Técnico",
                role = "User",
                connections = new List<string> { "DT89mQ4bFYHq7PpUOEfY9j" }
            },
            new User
            {
                userId = 10659,
                username = "FISCALIA\\fstachiotti",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/10659",
                mail = "stachiotti@fepba.gov.ar",
                fullname = "Stacchiotti, Fabián César",
                position = "Subsecretario",
                role = "User",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9k", "DT19mQ4bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 10660,
                username = "FISCALIA\\mbailheres",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/10660",
                mail = "mbailheres@fepba.gov.ar",
                fullname = "Bailheres, Maria Julia",
                position = "Lic. en Comunicación Social/Jefa de Departamento",
                role = "User",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 10665,
                username = "FISCALIA\\cpierrard",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/10665",
                mail = "cpierrard@fepba.gov.ar",
                fullname = "Pierrard, Cintia",
                position = "Secretaria",
                role = "Admin",
                connections = new List<string> { "DT32mQ4bFYHq7PpUOEfY9k", "DT19mQ4bFYHq7PpUOEfY9l" }
            },
            new User
            {
                userId = 10686,
                username = "FISCALIA\\acorredera",
                picture = "https://serviciosdev.fepba.gov.ar/directorio/api/Avatar/user/10686",
                mail = "acorredera@fepba.gov.ar",
                fullname = "Corredera, Agostina Paula",
                position = "Administrativa",
                role = "User",
                connections = new List<string> { "DT39mQ5bFYHq7PpUOEfY9l" }
            }
        };
    }
}

