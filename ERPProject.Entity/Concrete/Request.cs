using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class Request : BaseEntity
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long? AcceptedId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public short QuantityUnit { get; set; }
    public short RequestStatus { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual User AcceptedUser { get; set; }
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
