using connected_hub_api.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace connected_hub_api.IService;
public interface IUrlConnectionService
{

    ConcurrentDictionary<string, List<string>> GetAllUrlsWithConnections();
    Task<List<string>> GetListConnectionsByUrlAsync(string url);
    Task RemoveCurrentConnectionFromUrl(string connectionId, string url);
    Task SetCurrentConnectionToUrlAsync(Payload payload);

}