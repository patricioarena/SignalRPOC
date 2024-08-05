using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace session_api.Model
{
    [Serializable]
    public class UserConnection
    {
        public int userId { get; set; }
        public string? connectionId { get; set; }
    }
}
