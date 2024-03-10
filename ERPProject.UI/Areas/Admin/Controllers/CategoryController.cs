using ERPProject.Entity.DTO.CategoryDTO;
using ERPProject.Entity.Poco;
using ERPProject.UI.Areas.Admin.Models;
using ERPProject.UI.Areas.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter(Role = "Admin,Satın Alma İşlemleri,Ürün Görüntüleme")]
    public class CategoryController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public CategoryController(HttpClient httpClient) : base(httpClient)
        {
        }
        [HttpGet("/Admin/Kategoriler")]
        public async Task<IActionResult> Index()
        {

            var val = await GetAllAsync<CategoryDTOResponse>(url + "GetCategories");
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return View(val);
        }
        [HttpGet("/Admin/Kategori")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<CategoryDTOResponse>(url + "GetCategory/" + id);
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return View(val);
        }
        [HttpPost("/Admin/KategoriEkle")]
        public async Task<IActionResult> Add(CategoryDTORequest p)
        {
            p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await AddAsync(p, url + "AddCategory");
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data != null)
            {
                return RedirectToAction("Index", "Category");

            }
            return View(val);
        }
        [HttpPost("/Admin/KategoriGuncelle")]
        public async Task<IActionResult> Update(CategoryDTORequest p)
        {
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateCategory");
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val.Data != null)
            {
                return RedirectToAction("Index", "Category");

            }
            return View(val);
        }
        [HttpGet("/Admin/KategoriSil/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveCategory/" + id);
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val)
            {
                return RedirectToAction("Index", "Category");

            }
            return RedirectToAction("Forbidden", "Home");
        }
    }
}
