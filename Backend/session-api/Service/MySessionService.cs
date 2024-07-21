using session_api.IService;
using session_api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Policy;

namespace session_api.Service
{
    public class MySessionService : IMySessionService
    {
        private ConcurrentDictionary<int, User> userList = new ConcurrentDictionary<int, User>()
        {
            [3456] = new User
            {
                userId = 3456,
                username = "Erik",
                picture = "https://i.pinimg.com/736x/75/2d/0b/752d0bc66695c9dacd6858d38adeaec4.jpg",
                sessions = new List<string> { "-eswoeZl3ao8hLANGQwZEQ" }
            },
            [6788] = new User 
            { 
                userId = 6788, 
                username = "Charles", 
                picture = "https://i.pinimg.com/736x/ea/23/51/ea23510c375c824096adb31b127a6064.jpg",
                sessions = new List<string> { "H_KEV01cQrFzJdBN-Fx6lA" }
            }
        };

        private ConcurrentDictionary<string, List<string>> urlListSession = new ConcurrentDictionary<string, List<string>>()
        {
            ["http://localhost:4200/"] = new List<string> { "-eswoeZl3ao8hLANGQwZEQ" , "H_KEV01cQrFzJdBN-Fx6lA" },
            ["http://localhost:4201/"] = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "H_KEV01cQrFzJdBN-Fx6lA" }
        };

        public MySessionService() { }

        public void SetUserSession(UserSession userSession)
        {
            var existingUser = GetUserSessionByUserId(userSession.userId);
            if (existingUser != null)
            {
                UpdateExistingUserSession(userSession, userSession.connectionId);
            }
            else
            {
                AddNewUserSession(userSession);
            }
        }

        public User GetUserSessionByUserId(int userId)
        {
            return userList.TryGetValue(userId, out User userSession) ? userSession : null;
        }

        public List<string> GetListSession(string url)
        {
            return urlListSession.TryGetValue(url, out List<string> listSession) ? listSession : null;
        }

        public ConcurrentDictionary<int, User> GetUsersSessions()
        {
            return userList;
        }

        public ConcurrentDictionary<string, List<string>> GetUrlListSession()
        {
            return urlListSession;
        }

        public bool RemoveUserSession(UserSession userSession)
        {
            var existingUser = GetUserSessionByUserId(userSession.userId);
            if (existingUser != null)
            {
                return RemoveSessionFromUser(existingUser, userSession.connectionId);
            }
            return false;
        }

        public void UpdateExistingUsertWithPayload_TEST(Payload payload)
        {
            var existingList = GetListSession(payload.url);
            if (existingList != null)
            {
                if(!urlListSession[payload.url].Contains(payload.connectionId)){
                    urlListSession[payload.url].Add(payload.connectionId);
                }
            }
        }

        public void UpdateExistingUsertWithPayload(Payload payload)
        {
            var existingUser = GetUserSessionByUserId(payload.userId);
            if (existingUser != null)
            {
                if (String.IsNullOrEmpty(userList[payload.userId].username))
                    userList[payload.userId].username = payload.username;

                if (String.IsNullOrEmpty(userList[payload.userId].picture))
                    userList[payload.userId].picture = payload.picture;
            }
        }

        private void UpdateExistingUserSession(UserSession existingUserSession, string connectionId)
        {
            userList[existingUserSession.userId].sessions.Add(connectionId);
        }

        private void AddNewUserSession(UserSession userSession)
        {
            var newUser = new User { userId = userSession.userId, sessions = new List<string> { userSession.connectionId } };
            userList.TryAdd(newUser.userId, newUser);
        }

        private bool RemoveSessionFromUser(User user, string connectionId)
        {
            var index = user.userId;
            user.sessions.Remove(connectionId);

            return user.sessions.Count == 0
                ? userList.TryRemove(index, out _)
                : false;
        }
    }
}
