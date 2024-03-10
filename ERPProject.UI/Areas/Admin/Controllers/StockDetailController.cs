using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.StockDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.UI.Areas.Admin.Models;
using ERPProject.UI.Areas.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter(Role = "Admin,Satın Alma İşlemleri,Ürün Görüntüleme")]
    public class StockDetailController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        public StockDetailController(HttpClient httpClient) : base(httpClient)
        {

        }
        [HttpGet("/Admin/StokDetaylar")]
        public async Task<IActionResult> Index()
        {
            var val = await GetAllAsync<StockDetailDTOResponse>(url + "StockDetails");
            var val1 = await GetAllAsync<StockDTOResponse>(url + "Stocks");
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
            StockDetailVM stockDetailVM = new StockDetailVM
            {
                StockDetails = val.Data,
                Stocks = val1.Data
            };
            return View(stockDetailVM);
        }
        [HttpGet("/Admin/StokDetay")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<StockDetailDTOResponse>(url + "StockDetail/" + id);
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
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val == null)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost("/Admin/StokDetayEkle")]
        public async Task<IActionResult> Add(StockDetailDTORequest p)
        {
            p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            p.DelivererId = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var deliuser= await GetAsync<UserDTOResponse>(url + "GetUser/" + p.DelivererId);
            p.DelivererName = deliuser.Data.Name+ " " +deliuser.Data.LastName;
            var reciuser= await GetAsync<UserDTOResponse>(url + "GetUser/" + p.RecieverId);
            p.RecieverName = reciuser.Data.Name+ " " +reciuser.Data.LastName;


            var val = await AddAsync(p, url + "AddStockDetail");
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
            TempData["stok"] = "Stok Yetersiz...";
            return RedirectToAction("Index", "Stock");
        }
        [HttpPost("/Admin/StokDetayGuncelle")]
        public async Task<IActionResult> Update(StockDetailDTORequest p)
        {
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await UpdateAsync(p, url + "UpdateStockDetail");
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
        [HttpPost("/Admin/StokDetaySil")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveStockDetail/" + id);
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Satın Alma" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val)
            {
                return RedirectToAction("Index", "StockDetail");

            }
            return RedirectToAction("Index", "Home");

        }
    }
}
