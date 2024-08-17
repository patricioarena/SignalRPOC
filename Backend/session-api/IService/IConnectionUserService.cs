using session_api.Model;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace session_api.IService;
public interface IConnectionUserService
{
    Task<UserUrl> GetUserUrlByConnectionId(string connectionId);
    ConcurrentDictionary<string, UserUrl> GetAllConnectionUserMappings();
    Task AddMapConnectionIdUserId(Payload payload);
    Task RemoveCurrentConnectionFromConnectionUser(string connectionId);
}