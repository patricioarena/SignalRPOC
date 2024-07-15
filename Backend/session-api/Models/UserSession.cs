using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace session_api.Models
{
    [Serializable]
    public class UserSession
    {
        public string? username { get; set; }
        public string? connectionId { get; set; }
        public List<string> sessions { get; set; } = new List<string>();
    }
}
