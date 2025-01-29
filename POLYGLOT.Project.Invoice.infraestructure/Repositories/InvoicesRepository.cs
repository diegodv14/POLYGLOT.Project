using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Invoice.application.Dto;
using POLYGLOT.Project.Invoice.application.Interfaces;
using POLYGLOT.Project.Invoice.application.Models;
using POLYGLOT.Project.Invoice.application.Exceptions;

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
                InvoiceResponse res = new();
                var invoices = await _context.Invoices.ToListAsync();
                res.Invoices = invoices;
                return res;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public async Task<ResponseSuccess> AddInvoice(AddInvoiceDto data)
        {
            try
            {
                var secuencialExist = await _context.Invoices.FirstOrDefaultAsync(s => s.Secuencial == data.Secuencial);

                if (secuencialExist != null) throw new BaseCustomException($"Ya existe una factura con este secuencial {data.Secuencial}", "", 409);

                var newInvoice = new POLYGLOT.Project.Invoice.application.Models.Invoice()
                {
                    Amount = (float)data.Amount,
                    Secuencial = data.Secuencial,
                    Description = data.Description,
                    State = false
                };

                _context.Invoices.Add(newInvoice);
                await _context.SaveChangesAsync();

                return new ResponseSuccess()
                {
                    Message = "Factura creada exitosamente",
                    Status = 200
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<POLYGLOT.Project.Invoice.application.Models.Invoice> CheckInvoice(int secuencial)
        {
            try
            {
                var invoice = await _context.Invoices.FirstOrDefaultAsync(s => s.IdInvoice == secuencial) ?? throw new BaseCustomException($"La factura con secuencial {secuencial} no esta registrada.", "", 404);
                return invoice;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
