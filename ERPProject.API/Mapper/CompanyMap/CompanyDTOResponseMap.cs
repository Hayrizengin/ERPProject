using AutoMapper;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.CompanyMap
{
    public class CompanyDTOResponseMap:Profile
    {
        public CompanyDTOResponseMap()
        {
            CreateMap<Company, CompanyDTOResponse>();
            CreateMap<CompanyDTOResponse, Company>();
        }
    }
}
