using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.InvoiceDTO;
using ERPProject.Entity.DTO.OfferDTO;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.UserDTO;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class OfferVM
    {
        public virtual ICollection<UserDTOResponse> Users { get; set; } = new List<UserDTOResponse>();
        public virtual ICollection<OfferDTOResponse> Offers { get; set; } = new List<OfferDTOResponse>();
        public virtual ICollection<RequestDTOResponse> Requests { get; set; } = new List<RequestDTOResponse>();

    }
}
