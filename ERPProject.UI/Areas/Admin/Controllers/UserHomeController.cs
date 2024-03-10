using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserHomeController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public UserHomeController(HttpClient httpClient) : base(httpClient)
        {
        }

        [HttpGet("/Admin/Anasayfa")]
        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await GetAsync<UserDTOResponse>(url+ "GetUser/"+userId);
            var val1 = await GetAllAsync<RequestDTOResponse>(url+ "RequestsByUser/" + userId);
            var val2 = await GetAllAsync<StockDetailDTOResponse>(url+ "StockDetailsByUser/" + userId);

            UserInfoVM userDTO = new()
            {
                CompanyName=val.Data.CompanyName,
                DepartmentName=val.Data.DepartmentName,
                Name=val.Data.Name,
                LastName=val.Data.LastName,
                Image=val.Data.Image,

                RoleName=val.Data.RoleName.ToList(),
                Email=val.Data.Email,
                Phone=val.Data.Phone,
                Requests=val1.Data,
                StockDetail=val2.Data
            };


            return View(userDTO);
        }
    }
}
