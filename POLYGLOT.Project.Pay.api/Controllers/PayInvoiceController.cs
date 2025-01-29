﻿using Microsoft.AspNetCore.Mvc;
using POLYGLOT.Project.Pay.application.Dto;
using POLYGLOT.Project.Pay.application.Exceptions;
using POLYGLOT.Project.Pay.application.Interfaces;

namespace POLYGLOT.Project.Pay.api.Controllers
{
    public class PayInvoiceController : ControllerBase
    {
        private readonly IPayInvoice _pay;
        public PayInvoiceController(IPayInvoice pay) 
        { 
           _pay = pay;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseSuccess), 200)]
        [ProducesResponseType(typeof(ResponseError), 500)]
        [ProducesResponseType(typeof(ResponseError), 404)]
        [Route("/PolyGlot/Pay/DoPay")]
        public async Task<ActionResult<ResponseSuccess>> DoPay([FromBody] PayInvoiceRequest data)
        {
            var res = await _pay.HandlePayInvoice(data);
            return Ok(res);
        }
    }
}
