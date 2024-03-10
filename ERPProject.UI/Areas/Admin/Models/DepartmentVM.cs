using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.DepartmentDTO;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class DepartmentVM
    {
        public virtual ICollection<DepartmentDTOResponse> Departments { get; set; } = new List<DepartmentDTOResponse>();
    }
}
