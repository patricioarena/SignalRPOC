using System;

#nullable enable
namespace session_api.Model
{
    [Serializable]
    public record UserUrl(int UserId, string Url);
}
