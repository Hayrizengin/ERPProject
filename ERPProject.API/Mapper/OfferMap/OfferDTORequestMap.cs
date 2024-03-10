using AutoMapper;
using ERPProject.Entity.DTO.OfferDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.OfferMap
{
    public class OfferDTORequestMap : Profile
    {
        public OfferDTORequestMap()
        {
            CreateMap<Offer, OfferDTORequest>();
            CreateMap<OfferDTORequest, Offer>();
        }
    }
}
