using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Invoice.application.Dto;
using POLYGLOT.Project.Invoice.application.Interfaces;
using POLYGLOT.Project.Invoice.application.Models;
using POLYGLOT.Project.Invoice.application.Exceptions;
using System.Linq.Expressions;

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


        public async Task<POLYGLOT.Project.Invoice.application.Models.Invoice> CheckInvoice(int idInvoice)
        {
            try
            {
                var invoice = await _context.Invoices.FirstOrDefaultAsync(s => s.IdInvoice == idInvoice) ?? throw new BaseCustomException($"La factura con id {idInvoice} no esta registrada.", "", 404);
                return invoice;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
