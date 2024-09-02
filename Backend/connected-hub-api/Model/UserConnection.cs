using System;

#nullable enable
namespace connected_hub_api.Model
{
    [Serializable]
    public class UserConnection
    {
        public int userId { get; set; }
        public string? connectionId { get; set; }
    }
}
