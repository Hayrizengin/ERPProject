using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class Department : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CompanyId { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual Company Company { get; set; } = null!;
}
