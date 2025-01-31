using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POLYGLOT.Project.Transaction.api.Controllers.Base
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
    }
}
