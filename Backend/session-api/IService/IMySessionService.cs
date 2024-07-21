using session_api.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace session_api.IService
{
    public interface IMySessionService
    {
        User GetUserSessionByUserId(int userId);
        ConcurrentDictionary<int, User> GetUsersSessions();
        ConcurrentDictionary<string, List<string>> GetUrlListSession();
        bool RemoveUserSession(UserSession userSession);
        void SetUserSession(UserSession userSession);
        void UpdateExistingUsertWithPayload(Payload payload);
        void UpdateExistingUsertWithPayload_TEST(Payload payload);
    }
}
