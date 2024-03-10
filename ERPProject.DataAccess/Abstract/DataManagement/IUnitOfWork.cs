using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.DataAccess.Abstract.DataManagement
{
    public interface IUnitOfWork
    {
        IBrandRepository BrandRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IOfferRepository OfferRepository { get; }
        IProductRepository ProductRepository { get; }
        IRequestRepository RequestRepository { get; }
        IRoleRepository RoleRepository { get; }
        IStockRepository StockRepository { get; }
        IStockDetailRepository StockDetailRepository { get; }
        IUserRepository UserRepository { get; }
        IInvoiceDetailRepository InvoiceDetailRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
