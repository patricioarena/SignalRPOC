using session_api.Result;
using session_api.Service;

namespace session_api.Model;

public class Payload
{
    private string url1;

    private string picture1;

    public int userId { get; set; }

    public string username { get; set; }

    public string connectionId { get; set; }

    public string mail { get; set; }

    public string fullname { get; set; }

    public string position { get; set; }

    public string role { get; set; }

    public string pageUrl
    {
        get { return url1; }
        set { url1 = SetEncodeUrl(value); }
    }

    public string pictureUrl
    {
        get { return picture1; }
        set { picture1 = SetEncodeUrl(value); }
    }

    public string GetDecodeUrl() => Decode.Base64Url(this.url1);
    public string GetDecodePictureUrl() => Decode.Base64Url(this.picture1);

    public string SetEncodeUrl(string value)
    {
        if (Validator.IsValidBase64Url.Test(value))
            return value;

        if (Validator.IsValidUrl.Test(value))
            return value;

        throw new CustomException(CustomException.ErrorsEnum.Base64UrlNotFound);
    }
}