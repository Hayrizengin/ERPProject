using AutoMapper;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.ProductMap
{
    public class ProductDTOResponseMapper:Profile
    {
        public ProductDTOResponseMapper()
        {
            CreateMap<Product, ProductDTOResponse>().
                ForMember(dest => dest.BrandName, opt =>
                {
                    opt.MapFrom(src => src.Brand.Name);
                }).
                ForMember(dest => dest.CategoryName, opt =>
                {
                    opt.MapFrom(src => src.Category.Name);
                }).ReverseMap();
        }
    }
}
