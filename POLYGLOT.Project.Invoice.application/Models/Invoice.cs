using System;
using System.Collections.Generic;

namespace POLYGLOT.Project.Invoice.application.Models;

public partial class Invoice
{
    public int IdInvoice { get; set; }

    public float? Amount { get; set; }

    public bool? State { get; set; }

    public int Secuencial { get; set; }

    public string? Description { get; set; }
}
