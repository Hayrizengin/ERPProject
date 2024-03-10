using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class StockDetail : BaseEntity
{
    public long Id { get; set; }

    public long StockId { get; set; }

    public int Quantity { get; set; }
    public string RecieverName { get; set; }
    public string DelivererName { get; set; }

    public long RecieverId { get; set; }

    public long DelivererId { get; set; }

    public User User { get; set; }

    public virtual Stock Stock { get; set; } = null!;
}
