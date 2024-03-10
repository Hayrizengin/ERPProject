using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class Invoice : BaseEntity
{
    public DateTime InvoiceDate { get; set; }
    public long Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string SupplierName { get; set; }
    public string CompanyName { get; set; }
    public string PriceStatus { get; set; }
    public decimal Rate { get; set; }
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

}

