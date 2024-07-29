using session_api.IService;
using session_api.Models;
using session_api.Signal;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace session_api.Controllers
{
    [Route("api/session")]
    [EnableCors("AllowAll")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class SessionController : ControllerBase
    {
        private IHubContext<SignalHub> _hubContext;
        private IUserService _userService { get; set; }
        private IUrlConnectionService _urlConnectionService { get; set; }
        private IConnectionUserService _connectionUserService { get; set; }

        public SessionController(IHubContext<SignalHub> hubContext, IUserService userService, IUrlConnectionService urlConnectionService, IConnectionUserService connectionUserService)
        {
            _hubContext = hubContext;
            _userService = userService;
            _urlConnectionService = urlConnectionService;
            _connectionUserService = connectionUserService;
        }

        [HttpGet("get/all/users")]
        public IActionResult GetUsers()
        {
            var dir = _userService.GetAll();
            return Ok(dir);

        }

        [HttpGet("get/all/url/and/list/connections")]
        public IActionResult GetUrlListConnections()
        {
            var dir = _urlConnectionService.GetAll();
            return Ok(dir);
        }

        [HttpGet("get/all/connection/user")]
        public IActionResult GetConnectionUser()
        {
            var dir = _connectionUserService.GetAll();
            return Ok(dir);

        }

        [HttpGet("remove/connection/{connectionId}/of/user/{userId}")]
        public IActionResult RemoveCurrentConnection(string connectionId)
        {
            return Ok(connectionId);
        }
    }
}
