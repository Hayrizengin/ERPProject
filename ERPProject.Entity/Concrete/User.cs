using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class User : BaseEntity
{
    public long Id { get; set; }

    public int DepartmentId { get; set; }//gelecek

    public virtual Department Department { get; set; } = null!;

    //public int RoleId { get; set; }//gelecek

    //public virtual Role Role { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string Image { get; set; } = null!;


    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    public virtual ICollection<Request> AcceptedRequests { get; set; }=new List<Request>();

    public virtual ICollection<StockDetail> StockDetailDeliverers { get; set; } = new List<StockDetail>();
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual ICollection<StockDetail> StockDetailRecievers { get; set; } = new List<StockDetail>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();


}
