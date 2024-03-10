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
    public class InvoiceDetailManager : IInvoiceDetailService
    {
        private readonly IUnitOfWork _uow;

        public InvoiceDetailManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<InvoiceDetail> AddAsync(InvoiceDetail Entity)
        {
            await _uow.InvoiceDetailRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public async Task<IEnumerable<InvoiceDetail>> GetAllAsync(Expression<Func<InvoiceDetail, bool>> Filter = null, params string[] IncludeProperties)
        {
            return await _uow.InvoiceDetailRepository.GetAllAsync(Filter, IncludeProperties);
        }

        public async Task<InvoiceDetail> GetAsync(Expression<Func<InvoiceDetail, bool>> Filter, params string[] IncludeProperties)
        {
            return await _uow.InvoiceDetailRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(InvoiceDetail Entity)
        {
            Entity.IsActive = false;
            await _uow.InvoiceDetailRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(InvoiceDetail Entity)
        {
            await _uow.InvoiceDetailRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }
    }
}
