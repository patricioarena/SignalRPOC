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
        private readonly new Dictionary<int, UserSession> userList = new Dictionary<int, UserSession>() {
            [0] = new UserSession { username = "Erik", value = "-eswoeZl3ao8hLANGQwZEQ" },
            [1] = new UserSession { username = "Charles", value = "H_KEV01cQrFzJdBN-Fx6lA" }
        };


        public MySessionService() { }

        public void SetUserSession(UserSession userSession)
        {
            var test = this.GetUserSessionByUsername(userSession.username);
            if (test != null)
            {
                var index = userList.FirstOrDefault(x => x.Value.username == test.username).Key;
                userList[index] = userSession;
            }
            else
            {
                var index = userList.LastOrDefault().Key + 1;
                userList.Add(index, userSession);
            }
        }

        public bool RemoveUserSession(UserSession userSession)
        {
            var test = this.GetUserSessionByUsername(userSession.username);
            if (test != null)
            {
                var index = userList.FirstOrDefault(x => x.Value.username == test.username).Key;
                userList.Remove(index);
                return true;
            }
            return false;
        }

        public UserSession GetUserSessionByValue(string value)
        {
            var aUserSession =  userList.Where(e => e.Value.value == value).FirstOrDefault().Value;
            return aUserSession;
        }

        public UserSession GetUserSessionByIndex(int index)
        {
            return userList[index];
        }

        public UserSession GetUserSessionByUsername(string username)
        {
            var aUserSession =  userList.Where(e => e.Value.username == username).FirstOrDefault().Value;
            return aUserSession;

        }

        public Dictionary<int, UserSession> GetUsersSessions()
        {
            return userList;
        }

    }
}
