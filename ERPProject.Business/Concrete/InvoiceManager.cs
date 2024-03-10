using ERPProject.Business.Abstract;
using ERPProject.DataAccess.Abstract.DataManagement;
using ERPProject.Entity.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.Concrete
{
    public class InvoiceManager : IInvoiceService
    {
        private readonly IUnitOfWork _uow;

        public InvoiceManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Invoice> AddAsync(Invoice Entity)
        {
            await _uow.InvoiceRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync(Expression<Func<Invoice, bool>> Filter = null, params string[] IncludeProperties)
        {
            return await _uow.InvoiceRepository.GetAllAsync(Filter, IncludeProperties);
        }

        public async Task<Invoice> GetAsync(Expression<Func<Invoice, bool>> Filter, params string[] IncludeProperties)
        {
            return await _uow.InvoiceRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(Invoice Entity)
        {
            Entity.IsActive = false;
            await _uow.InvoiceRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(Invoice Entity)
        {
            await _uow.InvoiceRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }
    }
}
