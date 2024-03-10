using AutoMapper;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.DepartmentMap
{
    public class DepartmentDTORequestMap:Profile
    {
        public DepartmentDTORequestMap()
        {
            CreateMap<Department, DepartmentDTORequest>();
            CreateMap<DepartmentDTORequest, Department>();
        }
    }
}
