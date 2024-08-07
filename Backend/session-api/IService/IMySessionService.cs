﻿using session_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace session_api.IService
{
    public interface IMySessionService
    {
        void SetUserSession(UserSession userSession);
        bool RemoveUserSession(UserSession userSession);
        UserSession GetUserSessionByValue(string value);
        UserSession GetUserSessionByIndex(int index);
        UserSession GetUserSessionByUsername(string key);
        Dictionary<int, UserSession> GetUsersSessions();
    }
}
