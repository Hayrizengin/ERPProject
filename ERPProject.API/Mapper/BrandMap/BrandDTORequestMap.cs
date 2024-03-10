using AutoMapper;
using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.BrandMap
{
    public class BrandDTORequestMap : Profile
    {
        public BrandDTORequestMap()
        {
            CreateMap<Brand, BrandDTORequest>();
            CreateMap<BrandDTORequest, Brand>();
        }
    }
}
