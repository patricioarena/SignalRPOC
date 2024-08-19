using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using session_api.Contant;
using session_api.Signal;

namespace session_api.Controllers
{
    [Route("api/notification")]
    [EnableCors("AllowAll")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<SignalHub> _hubContext;

        public NotificationController(IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
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
            _hubContext.Clients.Client(connectionId).SendAsync(ClientMethod.Show_Notification, $"Send message to client {connectionId}");
            return Ok();
        }
    }
}
