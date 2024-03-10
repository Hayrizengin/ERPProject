using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class Offer : BaseEntity
{
    public long Id { get; set; }

    public string SupplierName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string PriceStatus { get; set; } = null!;

    public int Status { get; set; }
    public decimal Rate { get; set; }

    public long UserId { get; set; }//gelecek
    public virtual User User { get; set; } = null!;
    public long RequestId { get; set; }//gelecek
    public virtual Request Request { get; set; } = null!;

}
