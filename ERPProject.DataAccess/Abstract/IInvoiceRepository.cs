using ERPProject.Core.DataAccess;
using ERPProject.Entity.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.DataAccess.Abstract
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
    }
}
