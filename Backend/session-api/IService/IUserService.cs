using session_api.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.IService
{
    public interface IUserService
    {
        User GetUserByUserId(int userId);
        ConcurrentDictionary<int, User> GetUsers();
        Task RemoveUserSession(UserSession userSession);
        void SetUserSession(UserSession userSession);
        Task UpdateUserIfEmptyFields(Payload payload);
    }
}
