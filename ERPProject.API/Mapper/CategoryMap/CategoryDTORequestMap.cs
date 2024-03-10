using AutoMapper;
using ERPProject.Entity.DTO.CategoryDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.CategoryMap
{
    public class CategoryDTORequestMap :Profile
    {
        public CategoryDTORequestMap()
        {
            CreateMap<Category, CategoryDTORequest>();
            CreateMap<CategoryDTORequest, Category>();
        }
    }
}
