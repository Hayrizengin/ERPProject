using ERPProject.Entity.DTO.StockDTO;
using Microsoft.AspNetCore.Mvc;

using ERPProject.Entity.DTO.StockDTO;
using Microsoft.AspNetCore.Mvc;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.UI.Areas.Admin.Models;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using ERPProject.UI.Areas.Authentication;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter(Role = "Admin,Satın Alma İşlemleri,Ürün Görüntüleme")]
    public class StockController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public StockController(HttpClient httpClient) : base(httpClient)
        {

        }
        [HttpGet("/Admin/Stoklar")]
        public async Task<IActionResult> Index()
        {
            var val = await GetAllAsync<StockDTOResponse>(url + "Stocks");
            var val1 = await GetAllAsync<ProductDTOResponse>(url + "GetProducts");
            var val2 = await GetAllAsync<CompanyDTOResponse>(url + "GetCompanies");
            var val3 = await GetAllAsync<StockDetailDTOResponse>(url + "StockDetails");
            var val4 = await GetAllAsync<UserDTOResponse>(url + "GetUsers");

            if (!SessionRole.RoleName.Any(x => x.Contains("Admin")))
            {
                val = await GetAllAsync<StockDTOResponse>(url + "StocksByCompany/" + HttpContext.Session.GetString("Company"));
            }
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
            StockVM stockVM = new StockVM()
            {
                Companies = val2.Data,
                Products = val1.Data,
                Stocks = val.Data,
                StockDetails = val3.Data,
                Users = val4.Data
            };

            return View(stockVM);
        }
        [HttpGet("/Admin/Stok")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<StockDTOResponse>(url + "GetStock/" + id);
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
            return RedirectToAction("Index", "Stock");
        }
        [HttpPost("/Admin/StokEkle")]
        public async Task<IActionResult> Add(StockDTORequest p)
        {
            p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await AddAsync(p, url + "AddStock");
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
                return RedirectToAction("Index", "Stock");

            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpPost("/Admin/StokGuncelle")]
        public async Task<IActionResult> Update(StockDTORequest p)
        {

            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateStock");
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
                return RedirectToAction("Index", "Stock");

            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpGet("/Admin/StokSil/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveStock/" + id);
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
                return RedirectToAction("Index", "Stock");

            }
            return RedirectToAction("Forbidden", "Home");

        }
    }
}
