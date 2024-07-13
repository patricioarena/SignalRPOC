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
    public class NotificationsController : ControllerBase
    {
        private IHubContext<SignalHub> _hubContext;
        public IMySessionService _mySessionService { get; set; }

        public NotificationsController(IHubContext<SignalHub> hubContext, IMySessionService mySessionService)
        {
             _hubContext = hubContext;
            _mySessionService = mySessionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _hubContext.Clients.All.SendAsync("clientMethodName", "get all called");
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{connectionId}")]
        public ActionResult<string> Get(string connectionId)
        {
            _hubContext.Clients.Client(connectionId).SendAsync("clientMethodName", "get called");
            return "value";
        }

        [HttpGet("GetUsersSessions")]
        public Dictionary<int, UserSession> GetUsersSessions()
        {
            return _mySessionService.GetUsersSessions();
        }

        [HttpGet("RemoveUserSession/{connectionId}")]
        public bool RemoveUserSession(string connectionId)
        {
            var aSessionUser = _mySessionService.GetUserSessionByValue(connectionId);
            return _mySessionService.RemoveUserSession(aSessionUser);
        }
    }
}
