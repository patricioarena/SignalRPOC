using session_api.IService;
using session_api.Models;
using session_api.Signal;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace session_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class SessionController : ControllerBase
    {
        private IHubContext<SignalHub> _hubContext;
        public IMySessionService _mySessionService { get; set; }

        public SessionController(IHubContext<SignalHub> hubContext, IMySessionService mySessionService)
        {
            _hubContext = hubContext;
            _mySessionService = mySessionService;
        }

        [HttpGet("GetUsersSessions")]
        public IActionResult GetUsersSessions()
        {
            var dir = _mySessionService.GetUsersSessions();
            return Ok(dir);

        }

        [HttpGet("GetUrlListSession")]
        public IActionResult GetUrlListSession()
        {
            var dir = _mySessionService.GetUrlListSession();
            return Ok(dir);

        }

        //[HttpGet("RemoveUserSession/{connectionId}")]
        //public IActionResult RemoveUserSession(string connectionId)
        //{
        //    var aSessionUser = _mySessionService.GetUserSessionByConnectionId(connectionId);
        //    var flag = _mySessionService.RemoveUserSession(aSessionUser);
        //    return Ok(flag);
        //}
    }
}
