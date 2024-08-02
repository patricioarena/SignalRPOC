using session_api.IService;
using session_api.Models;
using session_api.Signal;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace session_api.Controllers
{
    [Route("api/notification")]
    [EnableCors("AllowAll")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class NotificationController : ControllerBase
    {
        private IHubContext<SignalHub> _hubContext;
        public IUserService _mySessionService { get; set; }

        public NotificationController(IHubContext<SignalHub> hubContext, IUserService mySessionService)
        {
            _hubContext = hubContext;
            _mySessionService = mySessionService;
        }

        [HttpGet("send/message/to/all")]
        public IActionResult Get()
        {
            _hubContext.Clients.All.SendAsync(ClientMethod.Show_Notification, "Send message to all clients");
            return Ok();
        }

        [HttpGet("send/message/to/connection/{connectionId}")]
        public IActionResult Get(string connectionId)
        {
            _hubContext.Clients.Client(connectionId).SendAsync(ClientMethod.Show_Notification, $"Send message to client { connectionId }");
            return Ok();
        }
    }
}
