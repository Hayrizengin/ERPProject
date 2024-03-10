using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.DTO.CategoryDTO;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.OfferDTO;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.Poco;
using ERPProject.UI.Areas.Admin.Models;
using ERPProject.UI.Areas.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter(Role = "Admin,Satın Alma İşlemleri,Ürün Görüntüleme")]
    public class ProductController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public ProductController(HttpClient httpClient) : base(httpClient)
        {

        }
        [HttpGet("/Admin/Urunler")]
        public async Task<IActionResult> Index()
        {
            var val = await GetAllAsync<ProductDTOResponse>(url + "GetProducts");
            var val2 = await GetAllAsync<BrandDTOResponse>(url + "GetBrands");
            var val3 = await GetAllAsync<CategoryDTOResponse>(url + "GetCategories");
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
            ProductVM productVM = new ProductVM()
            {
                Products = val.Data,
                Brands = val2.Data,
                Categories = val3.Data,
            };
            return View(productVM);
        }
        [HttpGet("/Admin/Urun")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<ProductDTOResponse>(url + "GetProduct/" + id);
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
        [HttpPost("/Admin/UrunEkle")]
        public async Task<IActionResult> Add(ProductDTORequest p)
        {
            p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await AddAsync(p, url + "AddProduct");
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
                return RedirectToAction("Index", "Product");

            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpPost("/Admin/UrunGuncelle")]
        public async Task<IActionResult> Update(ProductDTORequest p)
        {
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateProduct");
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
                return RedirectToAction("Index", "Product");

            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpGet("/Admin/UrunSil/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            var val = await DeleteAsync(url + "RemoveProduct/" + id);
            if (val)
            {
                return RedirectToAction("Index", "Product");

            }
            return RedirectToAction("Index", "Home");

        }
    }
}