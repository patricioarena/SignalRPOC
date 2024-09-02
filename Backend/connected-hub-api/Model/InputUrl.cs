using connected_hub_api.Service;

namespace connected_hub_api.Model;
public record InputUrl
{
    public string url;

    public InputUrl(string url) { this.url = Encode.Base64Url(url); }
}
