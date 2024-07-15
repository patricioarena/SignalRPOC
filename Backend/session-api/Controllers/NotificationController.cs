﻿using session_api.IService;
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
    public class NotificationController : ControllerBase
    {
        private IHubContext<SignalHub> _hubContext;
        public IMySessionService _mySessionService { get; set; }

        public NotificationController(IHubContext<SignalHub> hubContext, IMySessionService mySessionService)
        {
            _hubContext = hubContext;
            _mySessionService = mySessionService;
        }

        [HttpGet("send/message/to/all")]
        public IActionResult Get()
        {
            _hubContext.Clients.All.SendAsync("clientMethodName", "Send message to all clients");
            return Ok();
        }

        [HttpGet("send/message/to/connectionId/{connectionId}")]
        public IActionResult Get(string connectionId)
        {
            _hubContext.Clients.Client(connectionId).SendAsync("clientMethodName", $"Send message to client { connectionId }");
            return Ok();
        }
    }
}