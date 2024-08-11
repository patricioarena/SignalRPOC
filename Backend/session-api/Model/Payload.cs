using session_api.Service;

namespace session_api.Model;

public class Payload
{
    private string url1;
    private string picture1;

    public int userId { get; set; }
    public string username { get; set; }
    public string connectionId { get; set; }
    public string url
    {
        get => url1;
        set => url1 = Decode.Base64Url(value);
    }
    public string picture
    {
        get => picture1;
        set => picture1 = Decode.Base64Url(value);
    }
}