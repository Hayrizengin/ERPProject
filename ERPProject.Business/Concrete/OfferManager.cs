using AutoMapper;
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
    public class OfferManager : IOfferService
    {
        private readonly IUnitOfWork _uow;

        public OfferManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Offer> AddAsync(Offer Entity)
        {
            await _uow.OfferRepository.AddAsync(Entity);
            await _uow.SaveChangeAsync();
            return Entity;
        }

        public async Task<IEnumerable<Offer>> GetAllAsync(Expression<Func<Offer, bool>> Filter = null, params string[] IncludeProperties)
        {
            return await _uow.OfferRepository.GetAllAsync(Filter, IncludeProperties);
        }

        public async Task<Offer> GetAsync(Expression<Func<Offer, bool>> Filter, params string[] IncludeProperties)
        {
            return await _uow.OfferRepository.GetAsync(Filter, IncludeProperties);
        }

        public async Task RemoveAsync(Offer Entity)
        {
            Entity.IsActive = false;
            await _uow.OfferRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateAsync(Offer Entity)
        {
            await _uow.OfferRepository.UpdateAsync(Entity);
            await _uow.SaveChangeAsync();
        }
        public async Task<ICollection<Offer>> UpdateAllAsync(Offer Entity)
        { 
            var updateofferList = await _uow.OfferRepository.GetAllAsync(e => e.RequestId == Entity.RequestId);
            Entity.Status = 4;
            Entity.IsActive = true;
            await _uow.OfferRepository.UpdateAsync(Entity);

            if (updateofferList.ToList().Count==1)
            {
                await _uow.SaveChangeAsync();

                return updateofferList.ToList();
            }
            foreach (var val in updateofferList)
            {
                if (val.Id != Entity.Id)
                {
                    val.Status = 3;
                    await _uow.OfferRepository.UpdateAsync(val);
                }
                
            }
            await _uow.SaveChangeAsync();

            return updateofferList.ToList();
            //onay bekliyor 1
            //kabukl edildi 2
            //reddedildi 3
            //onaylandı 4
        }
    }
}
