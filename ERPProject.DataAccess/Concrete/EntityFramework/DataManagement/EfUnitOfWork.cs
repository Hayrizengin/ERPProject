using ERPProject.Core.Entity;
using ERPProject.DataAccess.Abstract;
using ERPProject.DataAccess.Abstract.DataManagement;
using ERPProject.DataAccess.Concrete.EntityFramework.Context;
using ERPProject.Entity.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.DataAccess.Concrete.EntityFramework.DataManagement
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ERPContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public EfUnitOfWork(IHttpContextAccessor contextAccessor, ERPContext context)
        {
            _contextAccessor = contextAccessor;
            _context = context;

            BrandRepository = new EfBrandRepository(_context);
            CategoryRepository = new EfCategoryRepository(_context);
            CompanyRepository = new EfCompanyRepository(_context);
            DepartmentRepository = new EfDepartmentRepository(_context);
            InvoiceRepository = new EfInvoiceRepository(_context);
            OfferRepository = new EfOfferRepository(_context);
            ProductRepository = new EfProductRepository(_context);
            RequestRepository = new EfRequestRepository(_context);
            RoleRepository = new EfRoleRepository(_context);
            StockRepository = new EfStockRepository(_context);
            StockDetailRepository = new EfStockDetailRepository(_context);
            UserRepository = new EfUserRepository(_context);
            InvoiceDetailRepository = new EfInvoiceDetailRepository(_context);
            UserRoleRepository = new EfUserRoleRepository(_context);
        }

        public IBrandRepository BrandRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public ICompanyRepository CompanyRepository { get; }

        public IDepartmentRepository DepartmentRepository { get; }

        public IInvoiceRepository InvoiceRepository { get; }

        public IOfferRepository OfferRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IRequestRepository RequestRepository { get; }

        public IRoleRepository RoleRepository { get; }

        public IStockRepository StockRepository { get; }

        public IStockDetailRepository StockDetailRepository { get; }

        public IUserRepository UserRepository { get; }
        public IInvoiceDetailRepository InvoiceDetailRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }

        public Task<int> SaveChangeAsync()
        {
            foreach (var item in _context.ChangeTracker.Entries<BaseEntity>())
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.AddedTime = DateTime.Now;
                    item.Entity.UpdatedTime = DateTime.Now;
                    item.Entity.AddedUser = item.Entity.AddedUser;
                    item.Entity.UpdatedUser = item.Entity.UpdatedUser;
                    item.Entity.AddedIPV4Address = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    item.Entity.UpdatedIPV4Address = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                    if (item.Entity.IsActive == null)
                    {
                        item.Entity.IsActive = true;
                    }

                }
                else if (item.State == EntityState.Modified)
                {
                    item.Entity.UpdatedTime = DateTime.Now;
                    item.Entity.UpdatedUser = item.Entity.UpdatedUser;
                    item.Entity.UpdatedIPV4Address = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                //else if (item.State == EntityState.Deleted)
                //{

                //    item.Entity.IsActive = false;
                //    item.State = EntityState.Modified;
                    
                //}

            }

            return _context.SaveChangesAsync();
        }
    }
}

