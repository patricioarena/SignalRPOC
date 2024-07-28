using session_api.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace session_api.IService;
public interface ISessionUserService
{
    ConcurrentDictionary<string, UserUrl> GetSessionsOfUsers();
    UserUrl GetUserIdByConnectionId(string connectionId);
    Task AddMapConnectionIdUserId(Payload payload);
}