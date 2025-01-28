using System;
using System.Collections;
using System.Collections.Generic;

namespace POLYGLOT.Project.Invoice.application.Models;

public partial class Invoice
{
    public int IdInvoice { get; set; }

    public decimal? Amount { get; set; }

    public BitArray? State { get; set; }
}
