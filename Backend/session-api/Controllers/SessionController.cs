using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using session_api.Core;
using session_api.IService;
using session_api.Model;
using session_api.Result;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace session_api.Controllers
{
    /// <summary>
    /// Para operar con los endpoints deberia: 
    /// [ConnectionId(IsEnabled = true)] validar siempre que tiene un connectionId.
    /// [ClientId(IsEnabled = true)] validar siempre que tiene un clientId.
    /// </summary>
    [Route("api/session")]
    [EnableCors("AllowAll")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class SessionController : CustomController
    {
        private readonly IUserService _userService;
        private readonly IUrlConnectionService _urlConnectionService;
        private readonly IConnectionUserService _connectionUserService;
        private readonly ILoggic _loggic;

        public SessionController(
            ILogger<SessionController> _logger,
            IUserService userService,
            IUrlConnectionService urlConnectionService,
            IConnectionUserService connectionUserService,
            ILoggic loggic) : base(_logger)
        {
            _userService = userService;
            _urlConnectionService = urlConnectionService;
            _connectionUserService = connectionUserService;
            _loggic = loggic;
        }

        /// <summary>
        /// Get all connected users.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches a list of all users who are currently connected.
        /// It can be used to monitor user activity and manage user sessions.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of connected users in JSON format.
        /// Returns a status code 200 if the operation is successful.
        /// </returns>
        [HttpGet("get/all/users/connected")]
        public IActionResult GetUsers()
        {
            var response = Response<ConcurrentDictionary<int, User>, object>.Builder()
                .SetStatusCode(HttpStatusCode.OK)
                .SetData(_userService.GetAllConnectedUsers())
                .Build();

            return Ok(response);
        }

        /// <summary>
        /// Get all URLs and their associated connections.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches a comprehensive list of URLs and the connections associated with each one.
        /// It can be used to monitor URL activity and manage URL connections.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of URLs with their corresponding connections
        /// in JSON format. Returns a status code 200 if the operation is successful.
        /// </returns>
        [HttpGet("get/all/urls/with/connections")]
        public IActionResult GetUrlListConnections()
        {
            var response = Response<ConcurrentDictionary<string, List<string>>, object>.Builder()
                .SetStatusCode(HttpStatusCode.OK)
                .SetData(_urlConnectionService.GetAllUrlsWithConnections())
                .Build();

            return Ok(response);
        }

        /// <summary>
        /// Get all connections and the corresponding user-to-URL mapping details.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches a list of all user connections and their associated URL mappings. 
        /// It can be used to audit user activities and verify URL assignments.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of connection-to-user mappings
        /// in JSON format. Returns a status code 200 if the operation is successful.
        /// </returns>
        [HttpGet("get/all/connection/user/mappings")]
        public IActionResult GetConnectionUser()
        {
            var response = Response<ConcurrentDictionary<string, UserUrl>, object>.Builder()
                .SetStatusCode(HttpStatusCode.OK)
                .SetData(_connectionUserService.GetAllConnectionUserMappings())
                .Build();

            return Ok(response);
        }

        /// <summary>
        /// Retrieves all users associated with the specified Base64-encoded URL.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches a list of all users corresponding to the provided Base64-encoded URL. 
        /// For example, `http://localhost:4200/` is encoded as `aHR0cDovL2xvY2FsaG9zdDo0MjAwLw`.
        /// The list of users returned will exclude any duplicates based on their URL access.
        /// Optionally, a specific user can be excluded from the results by providing their ID via the <paramref name="exclude"/> parameter.
        /// </remarks>
        /// <param name="base64URL">The Base64-encoded URL representing the target page.</param>
        /// <param name="exclude">
        /// An optional user ID to exclude from the list of returned users. 
        /// If provided, the user with this ID will not be included in the result.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a JSON-formatted list of users associated with the given URL. 
        /// Returns a status code 200 if the operation is successful.
        /// </returns>
        [HttpGet("get/user/for/pageUrl/{base64URL}")]
        public IActionResult GetConnectionUser(string base64URL, int? exclude = null)
        {
            var response = Response<List<User>, object>.Builder()
                .SetStatusCode(HttpStatusCode.OK)
                .SetData(_loggic.GetConnectionUserWithFiterAsync(base64URL, exclude).Result)
                .Build();

            return Ok(response);
        }

        /// <summary>
        /// Transform a URL to base64URL.
        /// </summary>
        /// <remarks>
        /// This endpoint transform a URL to base64URL, example:
        /// http://localhost:4200/  is aHR0cDovL2xvY2FsaG9zdDo0MjAwLw
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of users in pageUrl
        /// in JSON format. Returns a status code 200 if the operation is successful.
        /// </returns>
        [HttpPost("transform/pageUrl/to/base64URL")]
        public IActionResult TransformUrlToBase64Url([FromBody] InputUrl inputUrl)
        {
            return Ok(Response<InputUrl, object>.Builder()
                .SetStatusCode(HttpStatusCode.OK)
                .SetData(inputUrl)
                .Build());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("remove/connection/{connectionId}/of/user/{userId}")]
        public IActionResult RemoveCurrentConnection(string connectionId)
        {
            return Ok(connectionId);
        }
    }
}
