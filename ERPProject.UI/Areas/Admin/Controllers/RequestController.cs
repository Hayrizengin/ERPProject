using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.UI.Areas.Admin.Models;
using ERPProject.UI.Areas.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public RequestController(HttpClient httpClient) : base(httpClient)
        {
        }

        [HttpGet("/Admin/Talepler")]
        public async Task<IActionResult> Index()
        {
            var roles = SessionRole.RoleName;
            string dep = HttpContext.Session.GetString("DepartmentName");
            var id = HttpContext.Session.GetString("User");

            if (roles.Any(roles => roles.Contains("Personel")))
            {
                var val7 = await GetAllAsync<UserDTOResponse>(url + "GetUsers");
                var val5 = await GetAllAsync<ProductDTOResponse>(url + "GetProducts");
                var val6 = await GetAllAsync<RequestDTOResponse>(url + "RequestsByUser/" + id);

                RequestVM requestVM2 = new RequestVM()

                {
                    Users= val7.Data,
                    Requests = val6.Data,
                    Products = val5.Data,

                };
                return View(requestVM2);
            }
            var val = await GetAllAsync<RequestDTOResponse>(url + "Requests");

            if (roles.Any(roles => roles.Contains("Departman Müdürü")))
            {
                val = await GetAllAsync<RequestDTOResponse>(url + "RequestsByDepartment/" + id);

            }
            else if (roles.Any(roles => roles.Contains("Şirket Müdürü")))
            {
                val = await GetAllAsync<RequestDTOResponse>(url + "RequestsByCompany/" + id);

            }

            var val1 = await GetAllAsync<UserDTOResponse>(url + "GetUsers");
            var val2 = await GetAllAsync<ProductDTOResponse>(url + "GetProducts");
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            RequestVM requestVM = new RequestVM()

            {
                Requests = val.Data,
                Products = val2.Data,
                Users = val1.Data,
            };
            return View(requestVM);
        }
        [HttpGet("/Admin/Talep")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<RequestDTOResponse>(url + "Request/" + id);
            if (val.Data != null)
            {
                return View(val);
            }
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost("/Admin/TalepEkle")]
        public async Task<IActionResult> AddRequest(RequestDTORequest p)
        {
            p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await AddAsync(p, url + "AddRequest");
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data != null)
            {
                return RedirectToAction("Index", "Request");

            }
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Index", "Request");

        }
        [HttpPost("/Admin/TalepGuncelle")]
        public async Task<IActionResult> Update(RequestDTORequest p)
        {
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.AcceptedId = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateRequest");
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data != null)
            {
                return RedirectToAction("Index", "Request");

            }
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Index", "Request");

        }
        [HttpGet("/Admin/TalepSil/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveRequest/" + id);
            if (val)
            {
                return RedirectToAction("Index", "Request");

            }
            return RedirectToAction("Index", "Home");

        }

    }
}
