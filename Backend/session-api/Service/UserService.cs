using Microsoft.Extensions.Logging;
using session_api.IService;
using session_api.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.Service
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;

        private ConcurrentDictionary<int, User> users = new ConcurrentDictionary<int, User>()
        {
            [3456] = new User
            {
                userId = 3456,
                username = "NeutralPhoenix",
                picture = "https://i.pinimg.com/736x/75/2d/0b/752d0bc66695c9dacd6858d38adeaec4.jpg",
                mail = "neutralphoenix@example.com",
                fullname = "Erik Phoenix",
                position = "Software Engineer",
                role = "Admin",
                connections = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "-eswoeZl3ao8hLANGQwZdQ" }
            },
            [6788] = new User
            {
                userId = 6788,
                username = "TheProfesor",
                picture = "https://pm1.aminoapps.com/6437/03d7f5b0003df2e7a8b94ae5a5ef553548d344b6_00.jpg",
                mail = "theprofesor@marvel.com",
                fullname = "Charles Xavier",
                position = "Profesor",
                role = "Admin",
                connections = new List<string> { "H_KEV01cQrFzJdBN-Fx6lA", "H_KEV01cXrFzJdBN-Fx4lA" }
            }
        };

        public UserService(ILogger<UserService> logger) { _logger = logger; }

        public ConcurrentDictionary<int, User> GetAllConnectedUsers() => users;

        public async Task<User> GetUserByUserIdAsync(int userId)
        {
            return users.TryGetValue(userId, out User userSession)
                ? await Task.FromResult(userSession)
                : await Task.FromResult<User>(null);
        }

        public async Task SetCurrentConnection(UserConnection userConnection)
        {
            var existingUser = await GetUserByUserIdAsync(userConnection.userId);
            Func<Task> action = (existingUser == null)
                ? new Func<Task>(async () => await AddNewUserWithCurrentConnection(existingUser, userConnection))
                : new Func<Task>(async () => await UpdateExistingUserWithCurrentConnection(existingUser, userConnection));
            action();
        }

        private async Task AddNewUserWithCurrentConnection(User existingUser, UserConnection userConnection)
        {
            Func<Task> action = (existingUser == null)
                ? new Func<Task>(async () => await AddNewUser(userConnection))
                : new Func<Task>(async () => await Task.Yield());
            action();
        }

        private async Task AddNewUser(UserConnection userConnection) => await Task.Run(() =>
        {
            var newUser = buidUser(userConnection.userId, userConnection.connectionId);
            users.TryAdd(newUser.userId, newUser);
        });

        private static User buidUser(int userId, string connectionId) => new User { userId = userId, connections = new List<string> { connectionId } };

        private async Task UpdateExistingUserWithCurrentConnection(User existingUser, UserConnection userConnection)
        {
            Func<Task> action = (existingUser == null)
                ? new Func<Task>(async () => users[existingUser.userId].connections.Add(userConnection.connectionId))
                : new Func<Task>(async () => await Task.Yield());
            action();
        }

        public async Task UpdateUser(Payload payload)
        {
            var existingUser = await GetUserByUserIdAsync(payload.userId);
            Func<Task> action = (existingUser != null)
                ? new Func<Task>(async () => await UpdateUserIfEmptyFieldsAsync(payload, existingUser))
                : new Func<Task>(async () => await Task.Yield());
            await action();
        }

        private static async Task UpdateUserIfEmptyFieldsAsync(Payload payload, User existingUser)
        {
            async Task UpdateFieldsAsync()
            {
                if (string.IsNullOrEmpty(existingUser.username))
                    existingUser.username = payload.username;

                if (string.IsNullOrEmpty(existingUser.fullname))
                    existingUser.fullname = payload.fullname;

                if (string.IsNullOrEmpty(existingUser.mail))
                    existingUser.mail = payload.mail;

                if (string.IsNullOrEmpty(existingUser.picture))
                    existingUser.picture = payload.GetDecodePictureUrl();

                if (string.IsNullOrEmpty(existingUser.position))
                    existingUser.position = payload.position;

                if (string.IsNullOrEmpty(existingUser.role))
                    existingUser.role = payload.role;

            }
            await UpdateFieldsAsync();
        }

        public async Task RemoveCurrentConnectionFromUserAsync(UserConnection userConnection)
        {

            var existingUser = await GetUserByUserIdAsync(userConnection.userId);
            Func<Task> action = (existingUser != null)
                ? new Func<Task>(async () => await RemoveConnectionFromUser(existingUser, userConnection.connectionId))
                : new Func<Task>(async () => await Task.Yield());
            action();
        }

        public Task RemoveConnectionFromUser(User user, string connectionId)
        {
            return Task.FromResult(RemoveConnection(user, connectionId));
        }

        private bool RemoveConnection(User user, string connectionId)
        {
            var index = user.userId;
            var isSuccessRemoved = user.connections.Remove(connectionId);

            if (isSuccessRemoved && user.connections.Count > 0)
                return true;

            return user.connections.Count == 0
                ? users.TryRemove(index, out _)
                : false;
        }
    }
}
