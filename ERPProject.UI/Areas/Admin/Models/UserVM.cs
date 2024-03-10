using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.RoleDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.DTO.UserRoleDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class UserVM
    {

        public virtual ICollection<UserDTOResponse> Users { get; set; } = new List<UserDTOResponse>();
        public virtual ICollection<RequestDTOResponse>? Requests { get; set; } = new List<RequestDTOResponse>();
        public virtual ICollection<DepartmentDTOResponse> Departments { get; set; } = new List<DepartmentDTOResponse>();
        public virtual ICollection<CompanyDTOResponse> Companies { get; set; } = new List<CompanyDTOResponse>();
        public virtual ICollection<RoleDTOResponse> Roles { get; set; } = new List<RoleDTOResponse>();
        public virtual ICollection<UserRoleDTOResponse> UserRoles { get; set; } = new List<UserRoleDTOResponse>();
        



    }
}
