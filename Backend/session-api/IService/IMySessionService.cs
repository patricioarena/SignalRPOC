using session_api.Models;
using System.Collections.Concurrent;

namespace session_api.IService
{
    public interface IMySessionService
    {
        void SetUserSession(UserSession userSession);
        bool RemoveUserSession(UserSession userSession);
        UserSession GetUserSessionByConnectionId(string connectionId);
        UserSession GetUserSessionByUsername(string username);
        ConcurrentDictionary<int, UserSession> GetUsersSessions();
    }
}
