using AutoMapper;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.ProductMap
{
    public class ProductDTORequestMapper:Profile
    {
        public ProductDTORequestMapper()
        {
            CreateMap<Product,ProductDTORequest>();
            CreateMap<ProductDTORequest, Product>();
        }
    }
}
