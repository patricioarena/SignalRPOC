using session_api.Service;

namespace session_api.Model;
public record InputUrl
{
    public string url;

    public InputUrl(string url) { this.url = Encode.Base64Url(url); }
}
