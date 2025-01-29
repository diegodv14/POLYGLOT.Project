namespace POLYGLOT.Project.Pay.application.Dto
{
    public class InvoiceDto
    {
        public int idInvoice { get; set; }
        public decimal? amount { get; set; }          
        public int? secuencial { get; set; }
        public bool? state { get; set; }
        public decimal? paid { get; set; }

    }
}
