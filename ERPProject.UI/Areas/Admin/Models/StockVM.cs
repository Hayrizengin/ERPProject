using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.StockDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class StockVM
    {
        public virtual ICollection<StockDTOResponse> Stocks { get; set; } = new List<StockDTOResponse>();
        public virtual ICollection<ProductDTOResponse> Products { get; set; } = new List<ProductDTOResponse>();
        public virtual ICollection<CompanyDTOResponse> Companies { get; set; } = new List<CompanyDTOResponse>();
        public virtual ICollection<StockDetailDTOResponse> StockDetails { get; set; } = new List<StockDetailDTOResponse>();
        public virtual ICollection<UserDTOResponse> Users { get; set; } = new List<UserDTOResponse>();
    }
}
