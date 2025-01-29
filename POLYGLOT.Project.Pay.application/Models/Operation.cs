namespace POLYGLOT.Project.Pay.application.Models;

public partial class Operation
{
    public int IdOperation { get; set; }

    public int? IdInvoice { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Date { get; set; }
}
