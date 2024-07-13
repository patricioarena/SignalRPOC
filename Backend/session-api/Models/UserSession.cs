using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace session_api.Models
{
    [Serializable]
    public class UserSession
    {
        public string? username { get; set; }
        public string? value { get; set; }

    }
}
