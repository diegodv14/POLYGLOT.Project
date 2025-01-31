using Microsoft.AspNetCore.Mvc;
using POLYGLOT.Project.Invoice.api.Controllers.Base;
using POLYGLOT.Project.Invoice.application.Dto;
using POLYGLOT.Project.Invoice.application.Exceptions;
using POLYGLOT.Project.Invoice.application.Interfaces;

namespace POLYGLOT.Project.Invoice.api.Controllers
{
    public class InvoiceController : BaseController
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
        public async Task<ActionResult<InvoiceResponse>> GetInvoicesList()
        {
            var res = await _invoices.GetInvoices();
            return Ok(res);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseSuccess), 200)]
        [ProducesResponseType(typeof(ResponseError), 500)]
        [ProducesResponseType(typeof(ResponseError), 404)]
        [Route("/PolyGlot/Invoices/AddNewInvoice")]
        public async Task<ActionResult<ResponseSuccess>> AddNewInvoice([FromBody] AddInvoiceDto data)
        {
            var res = await _invoices.AddInvoice(data);
            return Ok(res);
        }


        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(POLYGLOT.Project.Invoice.application.Models.Invoice), 200)]
        [ProducesResponseType(typeof(ResponseError), 500)]
        [ProducesResponseType(typeof(ResponseError), 404)]
        [Route("/PolyGlot/Invoices/CheckInvoice")]
        public async Task<ActionResult<POLYGLOT.Project.Invoice.application.Models.Invoice>> CheckInvoiceStatus([FromQuery] int secuencial)
        {
            var res = await _invoices.CheckInvoice(secuencial);
            return Ok(res);
        }
    }
}
