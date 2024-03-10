using ERPProject.Entity.DTO.InvoiceDTO;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.UserDTO;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class RequestVM
    {
        public virtual ICollection<RequestDTOResponse> Requests { get; set; } = new List<RequestDTOResponse>();
        public virtual ICollection<UserDTOResponse> Users { get; set; } = new List<UserDTOResponse>();
        public virtual ICollection<ProductDTOResponse> Products { get; set; } = new List<ProductDTOResponse>();
    }
}
