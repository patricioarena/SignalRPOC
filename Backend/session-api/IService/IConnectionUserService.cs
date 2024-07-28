using session_api.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace session_api.IService;
public interface IConnectionUserService
{
    UserUrl GetUserIdByConnectionId(string connectionId);
    ConcurrentDictionary<string, UserUrl> GetAll();
    Task AddMapConnectionIdUserId(Payload payload);
}