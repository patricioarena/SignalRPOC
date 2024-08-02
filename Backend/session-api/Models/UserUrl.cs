using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace session_api.Models
{
    [Serializable]
    public class UserUrl
    {
        public int userId { get; set; }
        public string url { get; set; }
    }
}
