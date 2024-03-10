using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.DTO.CategoryDTO;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class ProductVM
    {
        public virtual ICollection<ProductDTOResponse> Products { get; set; } = new List<ProductDTOResponse>();
        public virtual ICollection<CategoryDTOResponse> Categories { get; set; } = new List<CategoryDTOResponse>();
        public virtual ICollection<BrandDTOResponse> Brands { get; set; } = new List<BrandDTOResponse>();

    }
}
