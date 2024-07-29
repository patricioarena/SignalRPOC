using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace session_api.Controllers
{
    [Route("api/ping")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public string Get() => "pong";
    }
}
