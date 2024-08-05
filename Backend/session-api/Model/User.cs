using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace session_api.Model
{
    [Serializable]
    public class User
    {
        public int userId { get; set; }
        public string? username { get; set; }
        public string? picture { get; set; }
        public List<string> connections { get; set; } = new List<string>();
    }
}
