using AutoMapper;
using ERPProject.Entity.DTO.StockDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.StockMap
{
    public class StockDTORequestMapper:Profile
    {
        public StockDTORequestMapper()
        {
            CreateMap<Stock,StockDTORequest>();
            CreateMap<StockDTORequest, Stock>();
        }
    }
}
