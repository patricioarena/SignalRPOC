using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using session_api.Result;
using System;
using System.Net;

namespace session_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomController : ControllerBase
    {
        public readonly ILogger<CustomController> _Logger;

        public CustomController(ILogger<CustomController> logger)
        {
            _Logger = logger;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ObjectResult CustomErrorStatusCode(Exception e)
        {
            if (e is CustomException)
            {
                var errorCode = ((CustomException)e).ErrorCode;
                var message = ((CustomException)e).Message;
                _Logger.LogError($"Error: {errorCode}, Message: {message}");
                return StatusCode((int)HttpStatusCode.PreconditionFailed, new Response<object>(HttpStatusCode.PreconditionFailed,
                    "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : message, errorCode));
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<object>(HttpStatusCode.InternalServerError,
                    "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : e.Message));
            }
        }
    }
}