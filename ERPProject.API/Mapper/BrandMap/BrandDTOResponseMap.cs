using AutoMapper;
using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.BrandMap
{
    public class BrandDTOResponseMap :Profile
    {
        public BrandDTOResponseMap()
        {
            CreateMap<Brand, BrandDTOResponse>();
            CreateMap<BrandDTOResponse, Brand>();
        }
    }
}
