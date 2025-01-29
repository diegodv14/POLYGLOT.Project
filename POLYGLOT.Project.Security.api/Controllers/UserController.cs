using Microsoft.AspNetCore.Mvc;
using POLYGLOT.Project.Security.application.Dto;
using POLYGLOT.Project.Security.application.Exceptions;
using POLYGLOT.Project.Security.application.Interfaces;

namespace POLYGLOT.Project.Security.api.Controllers
{
    [Tags("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        public UserController(IUser user) 
        {
          _user = user;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseSuccess), 200)]
        [ProducesResponseType(typeof(AuthError), 500)]
        [ProducesResponseType(typeof(AuthError), 404)]
        [Route("/PolyGlot/Security/CreateUser")]
        public async Task<ActionResult<ResponseSuccess>> CrearUsuario([FromBody] AuthRequest request)
        {
            var response = await _user.AddUser(request);
            return Ok(response);
        }
    }
}
