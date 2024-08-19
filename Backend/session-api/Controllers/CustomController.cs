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

                return StatusCode((int)HttpStatusCode.PreconditionFailed, Response<object, object>.Builder()
                    .SetStatusCode(HttpStatusCode.PreconditionFailed)
                    .SetMessage("Ha ocurrido un error")
                    .SetData(null)
                    .SetDeveloperMessage(e.InnerException != null ? e.InnerException.Message : message)
                    .SetErrorCode(errorCode)
                    .Build());
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, Response<object, object>.Builder()
                    .SetStatusCode(HttpStatusCode.InternalServerError)
                    .SetMessage("Ha ocurrido un error")
                    .SetData(null)
                    .SetDeveloperMessage(e.InnerException != null ? e.InnerException.Message : e.Message)
                    .Build());
            }
        }
    }
}