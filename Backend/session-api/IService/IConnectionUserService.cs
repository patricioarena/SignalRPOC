using session_api.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace session_api.IService;
public interface IConnectionUserService
{
    Task<UserUrl> GetUserUrlByConnectionId(string connectionId);
    ConcurrentDictionary<string, UserUrl> GetAll();
    Task AddMapConnectionIdUserId(Payload payload);
    Task RemoveCurrentConnectionFromConnectionUser(string connectionId);
}