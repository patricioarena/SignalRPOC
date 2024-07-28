using session_api.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace session_api.IService;

public interface IUrlSessionService
{
    List<string> GetListSession(string url);
    ConcurrentDictionary<string, List<string>> GetUrlListSession();
    Task AddConnectionToUserSessionIfNotExist(Payload payload);
}
