using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace POLYGLOT.Project.Transaction.application.Models
{
    public class TransactionModel
    {
        [BsonId]
        public ObjectId IdTransaccion { get; set; }
        public int IdInvoice { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set;}
    }
}
