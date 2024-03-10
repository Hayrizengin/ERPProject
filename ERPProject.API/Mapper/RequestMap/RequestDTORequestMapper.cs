using AutoMapper;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.RequestMap
{
    public class RequestDTORequestMapper:Profile
    {
        public RequestDTORequestMapper()
        {
            CreateMap<Request,RequestDTORequest>();
            CreateMap<RequestDTORequest, Request>();
        }
    }
}
