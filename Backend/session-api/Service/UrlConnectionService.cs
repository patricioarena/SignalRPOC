using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using session_api.IService;
using session_api.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace session_api.Service
{
    public class UrlConnectionService : IUrlConnectionService
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        private ConcurrentDictionary<string, List<string>> urlListConnections = new ConcurrentDictionary<string, List<string>>();
        public UrlConnectionService(ILogger<UrlConnectionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            UrlListConnectionsMock();
        }

        private void UrlListConnectionsMock()
        {
            ConcurrentDictionary<string, List<string>> urlListConnectionsMock = new ConcurrentDictionary<string, List<string>>()
            {
                ["http://localhost:4200/"] = new List<string> { "-eswoeZl3ao8hLANGQwZEQ", "H_KEV01cQrFzJdBN-Fx6lA" },
                ["http://localhost:4201/"] = new List<string> { "-eswoeZl3ao8hLANGQwZdQ", "H_KEV01cXrFzJdBN-Fx4lA" }
            };

            if (_configuration.GetValue<bool>("Mock:PreLoadData"))
                urlListConnections.Concat(urlListConnectionsMock);
        }

        public ConcurrentDictionary<string, List<string>> GetAllUrlsWithConnections() => urlListConnections;

        public async Task<List<string>> GetListConnectionsByUrlAsync(string url)
        {
            return urlListConnections.TryGetValue(url, out List<string> listConnection)
                ? await Task.FromResult(listConnection)
                : await Task.FromResult<List<string>>(null);
        }

        public async Task SetCurrentConnectionToUrlAsync(Payload payload)
        {
            var existingList = await GetListConnectionsByUrlAsync(payload.GetDecodeUrl());
            Func<Task> action = (existingList == null)
                ? new Func<Task>(async () => await AddConnectionToUrlAsync(existingList, payload))
                : new Func<Task>(async () => await UpdateConnectionInUrlAsync(existingList, payload));
            await action();
        }

        private async Task AddConnectionToUrlAsync(List<string> existingList, Payload payload)
        {
            Func<Task> action = (existingList == null)
                ? new Func<Task>(async () => await AddConnection(payload))
                : new Func<Task>(async () => await Task.Yield());
            action();
        }

        private async Task AddConnection(Payload payload) => await Task.Run(() =>
        {
            urlListConnections.TryAdd(payload.GetDecodeUrl(), new List<string> { payload.connectionId });
        });

        private async Task UpdateConnectionInUrlAsync(List<string> existingList, Payload payload)
        {
            Func<Task> action = (existingList != null) && noExistConnectionIdInList(existingList, payload.connectionId)
                ? new Func<Task>(async () => await Task.Run(() => existingList.Add(payload.connectionId)))
                : new Func<Task>(async () => await Task.Yield());
            action();
        }

        private static bool noExistConnectionIdInList(List<string> existingList, string connectionId) => !existingList.Contains(connectionId);

        public Task RemoveCurrentConnectionFromUrl(string connectionId, string url)
        {
            return Task.FromResult(RemoveConnectionFromUrl(connectionId, url));
        }

        private bool RemoveConnectionFromUrl(string connectionId, string url)
        {
            var urlList = urlListConnections[url];
            var isSuccessRemoved = urlList.Remove(connectionId);

            if (isSuccessRemoved && urlList.Count > 0)
                return true;

            return urlList.Count == 0
                ? urlListConnections.TryRemove(url, out _)
                : false;
        }
    }
}
