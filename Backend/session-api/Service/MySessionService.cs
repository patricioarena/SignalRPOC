using session_api.IService;
using session_api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace session_api.Service
{
    public class MySessionService : IMySessionService
    {
        private Dictionary<int, UserSession> userList = new Dictionary<int, UserSession>()
        {
            [0] = new UserSession { username = "Erik", connectionId = "-eswoeZl3ao8hLANGQwZEQ", sessions = new List<string> { "-eswoeZl3ao8hLANGQwZEQ" } },
            [1] = new UserSession { username = "Charles", connectionId = "H_KEV01cQrFzJdBN-Fx6lA", sessions = new List<string> { "H_KEV01cQrFzJdBN-Fx6lA" } }
        };

        public MySessionService() { }

        public void SetUserSession(UserSession userSession)
        {
            var existingUserSession = GetUserSessionByUsername(userSession.username);
            if (existingUserSession != null)
            {
                UpdateExistingUserSession(existingUserSession, userSession.connectionId);
            }
            else
            {
                AddNewUserSession(userSession);
            }
        }

        private void UpdateExistingUserSession(UserSession existingUserSession, string connectionId)
        {
            var index = userList.FirstOrDefault(x => x.Value.username == existingUserSession.username).Key;
            userList[index].sessions.Add(connectionId);
        }

        private void AddNewUserSession(UserSession userSession)
        {
            var index = userList.LastOrDefault().Key + 1;
            userSession.sessions.Add(userSession.connectionId);
            userList.Add(index, userSession);
        }

        public bool RemoveUserSession(UserSession userSession)
        {
            var existingUserSession = GetUserSessionByUsername(userSession.username);
            if (existingUserSession != null)
            {
                return RemoveSessionFromUser(existingUserSession, userSession.connectionId);
            }
            return false;
        }

        private bool RemoveSessionFromUser(UserSession userSession, string connectionId)
        {
            var index = userList.FirstOrDefault(x => x.Value.username == userSession.username).Key;
            userSession.sessions.Remove(connectionId);

            if (userSession.sessions.Count == 0)
            {
                userList.Remove(index);
            }

            return true;
        }

        public UserSession GetUserSessionByConnectionId(string connectionId)
        {
            return userList.Values.FirstOrDefault(x => x.connectionId == connectionId);
        }

        public UserSession GetUserSessionByUsername(string username)
        {
            return userList.Values.FirstOrDefault(x => x.username == username);
        }

        public Dictionary<int, UserSession> GetUsersSessions()
        {
            return userList;
        }
    }
}
