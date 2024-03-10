using AutoMapper;
using ERPProject.Entity.DTO.CategoryDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.CategoryMap
{
    public class CategoryDTOResponseMap : Profile
    {
        public CategoryDTOResponseMap()
        {
            CreateMap<Category,CategoryDTOResponse>();
            CreateMap<CategoryDTOResponse, Category>();
        }
    }
}
