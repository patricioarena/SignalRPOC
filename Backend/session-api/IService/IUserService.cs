using session_api.Model;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace session_api.IService
{
    public interface IUserService
    {
        User GetUserByUserId(int userId);
        ConcurrentDictionary<int, User> GetConnectedUsers();
        Task RemoveCurrentConnectionFromUser(UserConnection userConnection);
        void SetCurrentConnection(UserConnection userConnection);
        Task UpdateUserIfEmptyFields(Payload payload);
    }
}
