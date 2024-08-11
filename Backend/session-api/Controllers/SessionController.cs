using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using session_api.Core;
using session_api.IService;
using session_api.Model;
using session_api.Service;
using System.Collections.Generic;

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
    public class SessionController : ControllerBase
    {
        private IUserService _userService { get; set; }
        private IUrlConnectionService _urlConnectionService { get; set; }
        private IConnectionUserService _connectionUserService { get; set; }
        private ILoggic _loggic { get; set; }

        public SessionController(IUserService userService,
            IUrlConnectionService urlConnectionService,
            IConnectionUserService connectionUserService,
            ILoggic loggic)
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
            return Ok(_userService.GetConnectedUsers());
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
            return Ok(_urlConnectionService.GetAllUrlsWithConnections());
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
            return Ok(_connectionUserService.GetAllConnectionUserMappings());
        }

        /// <summary>
        /// Get all users for the corresponding base64URL.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches a list of all user for the corresponding base64URL. 
        /// http://localhost:4200/
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of users in url
        /// in JSON format. Returns a status code 200 if the operation is successful.
        /// </returns>
        [HttpGet("get/user/for/url/{base64URL}")]
        public IActionResult GetConnectionUser(string base64URL)
        {
            ///TODO: El mismo usuario puede tener la misma pagina en diferentes pestañas, navegadores, etc.
            /// Para la lista de usuarios que se debe retornar tomar en cuenta que no pueden haber usuarios
            /// repetidos para una misma pagina.

            var users = _loggic.GetUsersForUrl(Decode.Base64Url(base64URL)).Result;
            return Ok(users);
        }

        /// <summary>
        /// Transform a URL to base64URL.
        /// </summary>
        /// <remarks>
        /// This endpoint transform a URL to base64URL, example:
        /// http://localhost:4200/  is aHR0cDovL2xvY2FsaG9zdDo0MjAwLw
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of users in url
        /// in JSON format. Returns a status code 200 if the operation is successful.
        /// </returns>
        [HttpPost("transform/url/to/base64URL")]
        public IActionResult TransformUrlToBase64Url([FromBody] InputUrl inputUrl)
        {
            return Ok(new List<InputUrl> { inputUrl });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("remove/connection/{connectionId}/of/user/{userId}")]
        public IActionResult RemoveCurrentConnection(string connectionId)
        {
            return Ok(connectionId);
        }
    }
}
