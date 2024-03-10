using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.UserDTO;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class UserInfoVM
    {
        public string DepartmentName { get; set; }
        public List<string> RoleName { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Image { get; set; } = null!;
        public virtual ICollection<RequestDTOResponse>? Requests { get; set; } = new List<RequestDTOResponse>();
        public virtual ICollection<StockDetailDTOResponse>? StockDetail { get; set; } = new List<StockDetailDTOResponse>();
    }
}
