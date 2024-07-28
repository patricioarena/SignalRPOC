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
        private IUserService _userService { get; set; }
        private IUrlSessionService _urlSessionService { get; set; }
        private ISessionUserService _sessionUserService { get; set; }

        public SessionController(IHubContext<SignalHub> hubContext, IUserService mySessionService, IUrlSessionService urlSessionService, ISessionUserService sessionUserService)
        {
            _hubContext = hubContext;
            _userService = mySessionService;
            _urlSessionService = urlSessionService;
            _sessionUserService = sessionUserService;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var dir = _userService.GetUsers();
            return Ok(dir);

        }

        [HttpGet("GetUrlListSession")]
        public IActionResult GetUrlListSession()
        {
            var dir = _urlSessionService.GetUrlListSession();
            return Ok(dir);
        }

        [HttpGet("GetSessionsOfUsers")]
        public IActionResult GetSessionsOfUsers()
        {
            var dir = _sessionUserService.GetSessionsOfUsers();
            return Ok(dir);

        }

        //[HttpGet("RemoveUserSession/{connectionId}")]
        //public IActionResult RemoveUserSession(string connectionId)
        //{
        //    var aSessionUser = _userService.GetUserSessionByConnectionId(connectionId);
        //    var flag = _userService.RemoveUserSession(aSessionUser);
        //    return Ok(flag);
        //}
    }
}
