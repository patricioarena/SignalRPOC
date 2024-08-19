using System;
using System.Collections.Generic;
using JsonIgnore_1 = Newtonsoft.Json;
using JsonIgnore_2 = System.Text.Json.Serialization;

#nullable enable
namespace session_api.Model
{
    [Serializable]
    public class User
    {
        public int userId { get; set; }

        public string? username { get; set; }

        public string? picture { get; set; }

        public string? mail { get; set; }

        public string? fullname { get; set; }

        public string? position { get; set; }

        public string? role { get; set; }

        [JsonIgnore_1.JsonIgnore]
        [JsonIgnore_2.JsonIgnore(Condition = JsonIgnore_2.JsonIgnoreCondition.Always)]
        public List<string> connections { get; set; } = new List<string>();
    }
}
