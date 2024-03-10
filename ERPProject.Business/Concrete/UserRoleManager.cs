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
    public class UserRoleManager : IUserRoleService
    {
        private readonly IUnitOfWork _uow;

        public UserRoleManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<UserRole> AddAsync(UserRole Entity)
        {
            await _uow.UserRoleRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public Task<IEnumerable<UserRole>> GetAllAsync(Expression<Func<UserRole, bool>> Filter = null, params string[] IncludeProperties)
        {
            return _uow.UserRoleRepository.GetAllAsync(Filter, IncludeProperties);
        }

        public Task<UserRole> GetAsync(Expression<Func<UserRole, bool>> Filter, params string[] IncludeProperties)
        {
            return _uow.UserRoleRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(UserRole Entity)
        {
            await _uow.UserRoleRepository.RemoveAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(UserRole Entity)
        {
            await _uow.UserRoleRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }
    }
}
