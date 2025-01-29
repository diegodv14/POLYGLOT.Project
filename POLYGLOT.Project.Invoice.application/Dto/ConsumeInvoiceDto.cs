namespace POLYGLOT.Project.Invoice.application.Dto
{
    public class ConsumeInvoiceDto
    {
        public int idInvoice { get; set; }

        public float? amount { get; set; }

        public bool? state { get; set; }

        public int secuencial { get; set; }

        public string? description { get; set; }

        public float paid { get; set; }
    }
}
