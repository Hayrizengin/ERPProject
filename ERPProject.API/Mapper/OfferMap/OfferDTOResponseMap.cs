using AutoMapper;
using ERPProject.Entity.DTO.OfferDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.OfferMap
{
    public class OfferDTOResponseMap : Profile
    {
        public OfferDTOResponseMap()
        {
            CreateMap<Offer, OfferDTOResponse>().
                ForMember(dest => dest.UserName, opt =>
                {
                    opt.MapFrom(src => src.User.Name);
                })
                .ForMember(dest => dest.RequestName, opt =>
                {
                    opt.MapFrom(src=>src.Request.Title);
                }).ReverseMap();
        }
    }
}
