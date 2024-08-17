using Microsoft.AspNetCore.Mvc;

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
