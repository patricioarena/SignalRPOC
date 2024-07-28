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
                sessions = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "-eswoeZl3ao8hLANGQwZdQ" }
            },
            [6788] = new User 
            { 
                userId = 6788, 
                username = "Charles", 
                picture = "https://i.pinimg.com/736x/ea/23/51/ea23510c375c824096adb31b127a6064.jpg",
                sessions = new List<string> { "H_KEV01cQrFzJdBN-Fx6lA", "H_KEV01cXrFzJdBN-Fx4lA" }
            }
        };

        public UserService() { }

        public void SetUserSession(UserSession userSession)
        {
            var existingUser = GetUserByUserId(userSession.userId);
            if (existingUser != null)
            {
                UpdateExistingUserSession(userSession, userSession.connectionId);
            }
            else
            {
                AddNewUserSession(userSession);
            }
        }

        public User GetUserByUserId(int userId)
        {
            return users.TryGetValue(userId, out User userSession) ? userSession : null;
        }

        public ConcurrentDictionary<int, User> GetUsers()
        {
            return users;
        }

        public Task RemoveUserSession(UserSession userSession)
        {
            var existingUser = GetUserByUserId(userSession.userId);
            if (existingUser != null)
            {
                if (RemoveSessionFromUser(existingUser, userSession.connectionId))
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

        private void UpdateExistingUserSession(UserSession existingUserSession, string connectionId)
        {
            users[existingUserSession.userId].sessions.Add(connectionId);
        }

        private void AddNewUserSession(UserSession userSession)
        {
            var newUser = new User { userId = userSession.userId, sessions = new List<string> { userSession.connectionId } };
            users.TryAdd(newUser.userId, newUser);
        }

        private bool RemoveSessionFromUser(User user, string connectionId)
        {
            var index = user.userId;
            user.sessions.Remove(connectionId);

            return user.sessions.Count == 0
                ? users.TryRemove(index, out _)
                : false;
        }
    }
}
