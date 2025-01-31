namespace POLYGLOT.Project.Pay.application.Dto
{
    public class TransaccionDto
    {
        public int? IdTransaccion { get; set; }
        public int IdInvoice { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

    }
}
