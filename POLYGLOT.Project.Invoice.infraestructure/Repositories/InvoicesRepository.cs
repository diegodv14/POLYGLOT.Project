using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Invoice.application.Exceptions;
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

        public async Task<dynamic> GetInvoices()
        {
            try
            {
                var invoices = await _context.Invoices.ToListAsync();
                return "";
            }
            catch (Exception ex) {
                throw ex;
            }
        }


        public async Task<bool> UpdateInvoice(int idInvoice)
        {
            try
            {
                var invoice = await _context.Invoices.FirstOrDefaultAsync(s => s.IdInvoice == idInvoice) ?? throw new BaseCustomException($"La Factura con id {idInvoice} no existe", "", 404);

                invoice.State = true;
                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
