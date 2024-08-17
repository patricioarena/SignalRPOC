using session_api.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.IService;
public interface IUrlConnectionService
{

    ConcurrentDictionary<string, List<string>> GetAllUrlsWithConnections();
    Task<List<string>> GetListConnectionsByUrlAsync(string url);
    Task RemoveCurrentConnectionFromUrl(string connectionId, string url);
    Task SetCurrentConnectionToUrlAsync(Payload payload);

}