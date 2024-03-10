using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.DTO.UserRoleDTO;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class UserRoleVM
    {
        public virtual ICollection<UserDTOResponse> Users { get; set; } = new List<UserDTOResponse>();
        public virtual ICollection<UserRoleDTOResponse> UserRoles { get; set; } = new List<UserRoleDTOResponse>();

    }
}
