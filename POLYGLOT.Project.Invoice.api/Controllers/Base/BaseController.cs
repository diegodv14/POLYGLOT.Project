﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POLYGLOT.Project.Invoice.api.Controllers.Base
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
    }
}
