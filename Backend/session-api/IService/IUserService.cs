using session_api.Model;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace session_api.IService
{
    public interface IUserService
    {
        Task<User> GetUserByUserIdAsync(int userId);
        ConcurrentDictionary<int, User> GetAllConnectedUsers();
        Task RemoveCurrentConnectionFromUserAsync(UserConnection userConnection);
        Task SetCurrentConnection(UserConnection userConnection);
        Task UpdateUser(Payload payload);
    }
}
