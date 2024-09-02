using System;

#nullable enable
namespace connected_hub_api.Model
{
    [Serializable]
    public record UserUrl(int UserId, string Url);
}
