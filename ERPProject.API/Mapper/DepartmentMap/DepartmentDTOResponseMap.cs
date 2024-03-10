using AutoMapper;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.DepartmentMap
{
    public class DepartmentDTOResponseMap:Profile
    {
        public DepartmentDTOResponseMap()
        {
            CreateMap<Department, DepartmentDTOResponse>().
                ForMember(dest => dest.CompanyName, opt =>
                {
                    opt.MapFrom(src => src.Company.Name);
                }).ReverseMap();
        }
    }
}
