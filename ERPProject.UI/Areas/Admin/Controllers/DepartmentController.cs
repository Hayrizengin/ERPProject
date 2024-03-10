using ERPProject.Entity.DTO.DepartmentDTO;
using Microsoft.AspNetCore.Mvc;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public DepartmentController(HttpClient httpClient) : base(httpClient)
        {

        }
        [HttpGet("/Admin/Departmanlar")]
        public async Task<IActionResult> Index()
        {
            var val = await GetAllAsync<DepartmentDTOResponse>(url + "GetDepartments");
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Index", "Company");
        }
        [HttpGet("/Admin/Departman")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<DepartmentDTOResponse>(url + "GetDepartment/" + id);
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
        [HttpPost("/Admin/DepartmanEkle")]
        public async Task<IActionResult> Add(DepartmentDTORequest p)
        {
            p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await AddAsync(p, url + "AddDepartment");
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data != null)
            {
                return RedirectToAction("Index", "Company");

            }
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Index", "Company");

        }
        [HttpPost("/Admin/DepartmanGuncelle")]
        public async Task<IActionResult> Update(DepartmentDTORequest p)
        {
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateDepartment");
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data != null)
            {
                return RedirectToAction("Index", "Company");

            }
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Index", "Company");

        }
        [HttpGet("/Admin/DepartmanSil/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveDepartment/" + id);
            if (val)
            {
                return RedirectToAction("Index", "Company");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
