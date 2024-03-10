using ERPProject.Business.Abstract;
using ERPProject.DataAccess.Abstract.DataManagement;
using ERPProject.Entity.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks; 
namespace ERPProject.Business.Concrete
{
    public class RequestManager : IRequestService
    {
        private readonly IUnitOfWork _uow;

        public RequestManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Request> AddAsync(Request Entity)
        {
            await _uow.RequestRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public async Task<IEnumerable<Request>> GetAllAsync(Expression<Func<Request, bool>> Filter = null, params string[] IncludeProperties)
        {
            return await _uow.RequestRepository.GetAllAsync(Filter, IncludeProperties);
        }

        public async Task<Request> GetAsync(Expression<Func<Request, bool>> Filter, params string[] IncludeProperties)
        {
            return await _uow.RequestRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(Request Entity)
        {
            Entity.IsActive = false;
            await _uow.RequestRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(Request Entity)
        {
            await _uow.RequestRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

    }
}
