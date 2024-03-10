using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.DTO.CategoryDTO;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class CategoryVM
    {
        public virtual ICollection<CategoryDTOResponse> Categories { get; set; } = new List<CategoryDTOResponse>();

    }
}
