namespace session_api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using session_api.Result;

    /// <summary>
    /// Defines the <see cref="PingController" />
    /// </summary>
    [Route("api/ping")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class PingController : CustomController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{CustomController}"/></param>
        public PingController(ILogger<CustomController> logger) : base(logger)
        {
        }

        /// <summary>
        /// The Get
        /// </summary>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        public IActionResult Get() => Ok(Response<string, object>.Builder()
            .SetData("pong")
            .Build());
    }
}