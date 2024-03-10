using AutoMapper;
using ERPProject.Entity.DTO.RoleDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.RoleMap
{
    public class RoleDTOResponseMapper:Profile
    {
        public RoleDTOResponseMapper()
        {
            CreateMap<Role,RoleDTOResponse>();
            CreateMap<RoleDTOResponse, Role>().
                ForMember(dest => dest.UserRoles, opt =>
                {
                    opt.MapFrom(src => src.Name);
                });
        }
    }
}
