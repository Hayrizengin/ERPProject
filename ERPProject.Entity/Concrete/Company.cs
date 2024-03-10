using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;

namespace ERPProject.Entity.Poco;

public partial class Company : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
