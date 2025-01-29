namespace POLYGLOT.Project.Pay.application.Dto
{
    public class InvoiceDto
    {
        public int idInvoice { get; set; }
        public decimal? amount { get; set; }          
        public string? description { get; set; }
        public int? secuencial { get; set; }
        public bool state { get; set; }

    }
}
