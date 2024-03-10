using AutoMapper;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.StockDetailMap
{
    public class StockDetailDTOResponseMapper:Profile
    {
        public StockDetailDTOResponseMapper()
        {
            CreateMap<StockDetail, StockDetailDTOResponse>().
                ForMember(dest => dest.ProductName, opt =>
            {
                opt.MapFrom(src => src.Stock.Product.Name);
            }).ReverseMap();
        }
    }
}
