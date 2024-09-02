using connected_hub_api.Model;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace connected_hub_api.IService
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
