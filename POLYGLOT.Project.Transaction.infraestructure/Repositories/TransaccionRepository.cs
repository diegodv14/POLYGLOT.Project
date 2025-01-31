using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using POLYGLOT.Project.Transaction.application.Dto;
using POLYGLOT.Project.Transaction.application.Interfaces;
using POLYGLOT.Project.Transaction.application.Models;

namespace POLYGLOT.Project.Transaction.infraestructure.Repositories
{
    public class TransaccionRepository : ITransaccion
    {
        private readonly IMongoDatabase _context;
        private readonly IConfiguration _configuration;
        public TransaccionRepository(IMongoDatabase context, IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<GetTransaccionResponse> GetTransaccionList()
        {

            GetTransaccionResponse res = new();
            try
            {
                var collection = _context.GetCollection<TransactionModel>(_configuration["MongoSettings:Collection"]);
                res.Transacciones = await collection.Find(_ => true).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
