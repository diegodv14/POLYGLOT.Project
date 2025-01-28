using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Invoice.application.Dto;
using POLYGLOT.Project.Invoice.application.Interfaces;
using POLYGLOT.Project.Invoice.application.Models;

namespace POLYGLOT.Project.Invoice.infraestructure.Repositories
{

    public class InvoicesRepository : IInvoices
    {

        private readonly DbInvoiceContext _context;

        public InvoicesRepository(DbInvoiceContext context)
        { 
            _context = context;
        }

        public async Task<InvoiceResponse> GetInvoices()
        {
            try
            {
                InvoiceResponse res = new InvoiceResponse();
                var invoices = await _context.Invoices.ToListAsync();
                res.Invoices = invoices;
                return res;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

    }
}
