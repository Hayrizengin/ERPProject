using AutoMapper;
using ERPProject.Entity.DTO.RoleDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.RoleMap
{
    public class RoleDTORequestMapper:Profile
    {
        public RoleDTORequestMapper()
        {
            CreateMap<Role,RoleDTORequest>();
            CreateMap<RoleDTORequest, Role>();
        }
    }
}
