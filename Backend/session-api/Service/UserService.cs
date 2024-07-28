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
    public class UserService : IUserService
    {
        private ConcurrentDictionary<int, User> users = new ConcurrentDictionary<int, User>()
        {
            [3456] = new User
            {
                userId = 3456,
                username = "Erik",
                picture = "https://i.pinimg.com/736x/75/2d/0b/752d0bc66695c9dacd6858d38adeaec4.jpg",
                connections = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "-eswoeZl3ao8hLANGQwZdQ" }
            },
            [6788] = new User 
            { 
                userId = 6788, 
                username = "Charles", 
                picture = "https://i.pinimg.com/736x/ea/23/51/ea23510c375c824096adb31b127a6064.jpg",
                connections = new List<string> { "H_KEV01cQrFzJdBN-Fx6lA", "H_KEV01cXrFzJdBN-Fx4lA" }
            }
        };

        public UserService() { }

        public ConcurrentDictionary<int, User> GetAll() => users;

        public void SetCurrentConnection(UserConnection userConnection)
        {
            var existingUser = GetUserByUserId(userConnection.userId);
            Action action = (existingUser != null)
                ? new Action(() => UpdateExistingUserWithCurrentConnection(userConnection, userConnection.connectionId))
                : new Action(() => AddNewUserWithCurrentConnection(userConnection));
            action();
        }

        public User GetUserByUserId(int userId)
        {
            return users.TryGetValue(userId, out User userSession) ? userSession : null;
        }

        public Task RemoveCurrentConnection(UserConnection userConnection)
        {
            var existingUser = GetUserByUserId(userConnection.userId);
            if (existingUser != null)
            {
                if (RemoveConnectionFromUser(existingUser, userConnection.connectionId))
                    return Task.CompletedTask;
                return Task.FromException(new InvalidOperationException());
            }
            return Task.FromException(new UserNotFoundException());
        }

        public Task UpdateUserIfEmptyFields(Payload payload)
        {
            try
            {
                var existingUser = GetUserByUserId(payload.userId);
                if (existingUser != null)
                {
                    if (string.IsNullOrEmpty(existingUser.username))
                        existingUser.username = payload.username;

                    if (string.IsNullOrEmpty(existingUser.picture))
                        existingUser.picture = payload.picture;

                    return Task.CompletedTask; // Representa éxito sin valor de retorno
                }
                else
                {
                    return Task.FromException(new UserNotFoundException());
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex); // Representa error en caso de excepción
            }
        }

        private void UpdateExistingUserWithCurrentConnection(UserConnection existingUserConnection, string connectionId)
        {
            users[existingUserConnection.userId].connections.Add(connectionId);
        }

        private void AddNewUserWithCurrentConnection(UserConnection userSession)
        {
            var newUser = new User { userId = userSession.userId, connections = new List<string> { userSession.connectionId } };
            users.TryAdd(newUser.userId, newUser);
        }

        private bool RemoveConnectionFromUser(User user, string connectionId)
        {
            var index = user.userId;
            user.connections.Remove(connectionId);

            return user.connections.Count == 0
                ? users.TryRemove(index, out _)
                : false;
        }
   }
}
