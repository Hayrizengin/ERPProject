﻿using ERPProject.Business.Abstract;
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
    public class BrandManager : IBrandService
    {
        private readonly IUnitOfWork _uow;

        public BrandManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Brand> AddAsync(Brand Entity)
        {
            await _uow.BrandRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>> Filter = null, params string[] IncludeProperties)
        {
            return await _uow.BrandRepository.GetAllAsync(Filter, IncludeProperties);
        }

        public async Task<Brand> GetAsync(Expression<Func<Brand, bool>> Filter, params string[] IncludeProperties)
        {
            return await _uow.BrandRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(Brand Entity)
        {
            Entity.IsActive = false;
            await _uow.BrandRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(Brand Entity)
        {
            await _uow.BrandRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }
    }
}
