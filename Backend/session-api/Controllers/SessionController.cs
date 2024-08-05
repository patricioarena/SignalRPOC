using session_api.IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using session_api.Core;

namespace session_api.Controllers
{
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
        /// Retrieves all connected users.
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
        /// Retrieves all URLs and their associated connections.
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
        /// Retrieves all connections and the corresponding user-to-URL mapping details.
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
        
        [HttpGet("get/user/for/url/{url}")]
        public IActionResult GetConnectionUser(string url)
        {
            System.Console.WriteLine(url);
            var users = _loggic.GetUsersForUrl();
            return Ok(users);
        }

        [HttpGet("remove/connection/{connectionId}/of/user/{userId}")]
        public IActionResult RemoveCurrentConnection(string connectionId)
        {
            return Ok(connectionId);
        }
    }
}
