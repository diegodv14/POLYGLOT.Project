using Microsoft.AspNetCore.Mvc;
using POLYGLOT.Project.Transaction.application.Dto;
using POLYGLOT.Project.Transaction.application.Exceptions;
using POLYGLOT.Project.Transaction.application.Interfaces;

namespace POLYGLOT.Project.Transaction.api.Controllers
{
    public class TransaccionController: ControllerBase
    {
        private readonly ITransaccion _transaccion;
        public TransaccionController(ITransaccion transaccion) 
        { 
           _transaccion = transaccion;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetTransaccionResponse), 200)]
        [ProducesResponseType(typeof(ResponseError), 500)]
        [ProducesResponseType(typeof(ResponseError), 404)]
        [Route("/PolyGlot/Transaccion/GetTransaccion")]
        public async Task<ActionResult<GetTransaccionResponse>> ObtenerTransacciones()
        {
            var response = await _transaccion.GetTransaccionList();
            return Ok(response);
        }
    }
}
