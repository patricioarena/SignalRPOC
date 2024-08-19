using session_api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.Core
{
    public interface ILoggic
    {
        void SetCurrentConnection(UserConnection userConnection);
        Task SynchronizeRemoveData(UserConnection userConnection);
        Task SynchronizeUpdateData(Payload payload);
        void LogTaskFaulted(string methodName, Task task);
        Task<List<User>> GetUsersForUrl(string url);
        Task<List<User>> GetConnectionUserWithFiterAsync(string base64URL, int? exclude);
        Task<User> GetUserByUserId(int userId);
    }
}

