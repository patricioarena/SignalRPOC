using session_api.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.IService;

public interface IUrlConnectionService
{
    List<string> GetListConnectionsByUrl(string url);
    ConcurrentDictionary<string, List<string>> GetAll();
    Task AddConnectionToListConnectionsIfNotExist(Payload payload);
    Task RemoveCurrentConnectionFromUrl(string connectionId, string url);
}
