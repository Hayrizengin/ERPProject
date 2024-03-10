using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.Poco;
using ERPProject.UI.Areas.Admin.Models;
using ERPProject.UI.Areas.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter(Role = "Admin,Satın Alma İşlemleri,Ürün Görüntüleme")]
    public class BrandController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public BrandController(HttpClient httpClient) : base(httpClient)
        {
        }
        [HttpGet("/Admin/Markalar")]
        public async Task<IActionResult> Index()
        {
            var val = await GetAllAsync<BrandDTOResponse>(url + "GetBrands");
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
        [HttpGet("/Admin/Marka")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<BrandDTOResponse>(url + "GetBrand/" + id);
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
        [HttpPost("/Admin/MarkaEkle")]
        public async Task<IActionResult> Add(BrandDTORequest p)
        {
            p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await AddAsync(p, url + "AddBrand");
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
                return RedirectToAction("Index", "Brand");

            }
            return RedirectToAction("Forbidden", "Home");
        }
        [HttpPost("/Admin/MarkaGuncelle")]
        public async Task<IActionResult> Update(BrandDTORequest p)
        {
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateBrand");
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
                return RedirectToAction("Index", "Brand");

            }
            return RedirectToAction("Forbidden", "Home");
        }
        [HttpGet("/Admin/MarkaSil/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveBrand/" + id);
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
                return RedirectToAction("Index", "Brand");
            }
            return RedirectToAction("Forbidden", "Home");
        }
    }
}