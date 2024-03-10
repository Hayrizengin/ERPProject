using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class Stock : BaseEntity
{
    public long Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
    public short QuantityUnit { get; set; }

    public string? Description { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<StockDetail> StockDetails { get; set; } = new List<StockDetail>();
}
