using AutoMapper;
using ERPProject.Entity.DTO.UserRoleDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.UserRoleMap
{
    public class UserRoleDTOResponseMapper:Profile
    {
        public UserRoleDTOResponseMapper()
        {
            CreateMap<UserRole, UserRoleDTOResponse>().ForMember(dest => dest.RoleName, opt =>
            {
                opt.MapFrom(src => src.Role.Name);
            }).
            ForMember(dest => dest.UserName, opt =>
            {
                opt.MapFrom(src => src.User.Name);
            }).ReverseMap();
        }
    }
}
