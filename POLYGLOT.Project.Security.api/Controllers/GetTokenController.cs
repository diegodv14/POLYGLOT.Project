using Microsoft.AspNetCore.Mvc;
using POLYGLOT.Project.Security.application.Exceptions;
using POLYGLOT.Project.Security.application.Interfaces;
using POLYGLOT.Project.Security.application.Models;

namespace POLYGLOT.Project.Security.api.Controllers
{
    [Tags("GetToken")]
    [ApiController]
    public class GetTokenController : ControllerBase
    {
        private readonly IGetToken _getToken;
        public GetTokenController(IGetToken getToken) 
        { 
          _getToken = getToken;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(AuthError), 500)]
        [ProducesResponseType(typeof(AuthError), 404)]
        [Route("/PolyGlot/Security/GetToken")]
        public async Task<ActionResult<AuthResponse>> GenerarToken([FromBody] AuthRequest request)
        {
            var response = await _getToken.GetToken(request);
            return Ok(response);
        }
    }
}
