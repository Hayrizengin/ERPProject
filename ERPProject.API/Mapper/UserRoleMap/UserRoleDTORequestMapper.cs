using AutoMapper;
using ERPProject.Entity.DTO.RoleDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.DTO.UserRoleDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.UserRoleMap
{
    public class UserRoleDTORequestMapper : Profile
    {
        public UserRoleDTORequestMapper()
        {
            CreateMap<UserRole, UserRoleDTORequest>();
            CreateMap<UserRoleDTORequest, UserRole>();
            
        }
    }
}
