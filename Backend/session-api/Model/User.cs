using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable enable
namespace session_api.Model
{
    [Serializable]
    public class User
    {
        public int userId { get; set; }
        public string? username { get; set; }
        public string? picture { get; set; }

        [JsonIgnore]
        public List<string> connections { get; set; } = new List<string>();
    }
}
