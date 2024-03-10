using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Core.Entity;
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
    public class UserManager :IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<User> AddAsync(User Entity)
        {
            await _uow.UserRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> Filter = null, params string[] IncludeProperties)
        {
            return await _uow.UserRepository.GetAllAsync(Filter, IncludeProperties);
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>> Filter, params string[] IncludeProperties)
        {
            return await _uow.UserRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(User Entity)
        {
            Entity.IsActive = false;
            await _uow.UserRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(User Entity)
        {
            await _uow.UserRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        
    }
}
