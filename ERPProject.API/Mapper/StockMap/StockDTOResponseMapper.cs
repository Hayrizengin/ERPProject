using AutoMapper;
using ERPProject.Entity.DTO.StockDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.StockMap
{
    public class StockDTOResponseMapper:Profile
    {
        public StockDTOResponseMapper()
        {
            CreateMap<Stock, StockDTOResponse>().
                ForMember(dest => dest.CompanyName, opt =>
                {
                    opt.MapFrom(src => src.Company.Name);
                }).
                ForMember(dest => dest.ProductName, opt =>
                {
                    opt.MapFrom(src => src.Product.Name);
                })
                .ReverseMap();
            
        }
    }
}
