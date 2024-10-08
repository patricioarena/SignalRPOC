﻿using connected_hub_api.Service;

namespace connected_hub_api.Model;

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
        set { url1 = value; }
    }

    public string pictureUrl
    {
        get { return picture1; }
        set { picture1 = value; }
    }

    public string GetDecodeUrl() => Decode.Base64Url(this.url1);

    public string GetDecodePictureUrl() => Decode.Base64Url(this.picture1);

}