using ERPProject.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Entity.Poco
{
    public partial class InvoiceDetail : BaseEntity
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public short QuantityUnit { get; set; }
        public string ProductName { get; set; }
        public Invoice Invoice { get; set; }
        public long InvoiceId { get; set; }
        public string PriceStatus { get; set; }
        public decimal Rate { get; set; }
    }
}
