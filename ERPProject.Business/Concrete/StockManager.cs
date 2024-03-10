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
    public class StockManager : IStockService
    {
        private readonly IUnitOfWork _uow;

        public StockManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Stock> AddAsync(Stock Entity)
        {
            await _uow.StockRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public async Task<IEnumerable<Stock>> GetAllAsync(Expression<Func<Stock, bool>> Filter = null, params string[] IncludeProperties)
        {
            return await _uow.StockRepository.GetAllAsync(Filter, IncludeProperties);
            
        }

        public async Task<Stock> GetAsync(Expression<Func<Stock, bool>> Filter, params string[] IncludeProperties)
        {
            return await _uow.StockRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(Stock Entity)
        {
            Entity.IsActive = false;
            await _uow.StockRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(Stock Entity)
        {
            await _uow.StockRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }
    }
}
