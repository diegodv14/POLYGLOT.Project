using Microsoft.AspNetCore.Mvc;
using POLYGLOT.Project.Invoice.application.Dto;
using POLYGLOT.Project.Invoice.application.Exceptions;
using POLYGLOT.Project.Invoice.application.Interfaces;

namespace POLYGLOT.Project.Invoice.api.Controllers
{
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoices _invoices;
        public InvoiceController(IInvoices invoices)
        {
            _invoices = invoices;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InvoiceResponse), 200)]
        [ProducesResponseType(typeof(ResponseError), 500)]
        [ProducesResponseType(typeof(ResponseError), 404)]
        [Route("/PolyGlot/Invoices/GetAllInvoices")]
        public async Task<ActionResult<dynamic>> GetInvoicesList()
        {
            var res = await _invoices.GetInvoices();
            return Ok(res);
        }
    }
}
