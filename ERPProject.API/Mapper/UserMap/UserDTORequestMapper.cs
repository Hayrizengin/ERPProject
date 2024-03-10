using AutoMapper;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.UserMap
{
    public class UserDTORequestMapper:Profile
    {
        public UserDTORequestMapper()
        {
            CreateMap<User, UserDTORequest>();
            CreateMap<UserDTORequest, User>();
        }
    }
}
