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
        void LogTaskError(string methodName, Task task);
        Task<List<User>> GetUsersForUrl(string url);
        List<User> GetConnectionUserWithFiter(string base64URL, int? exclude);
    }
}

