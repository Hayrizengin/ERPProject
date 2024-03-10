using AutoMapper;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.CompanyMap
{
    public class CompanyDTORequestMap:Profile
    {
        public CompanyDTORequestMap()
        {
            CreateMap<Company, CompanyDTORequest>();
            CreateMap<CompanyDTORequest, Company>();
        }
    }
}
