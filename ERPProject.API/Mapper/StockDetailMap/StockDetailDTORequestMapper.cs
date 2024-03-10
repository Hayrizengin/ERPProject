using AutoMapper;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.StockDetailMap
{
    public class StockDetailDTORequestMapper:Profile
    {
        public StockDetailDTORequestMapper()
        {
            CreateMap<StockDetail, StockDetailDTORequest>();
            CreateMap<StockDetailDTORequest, StockDetail>();
        }
    }
}
