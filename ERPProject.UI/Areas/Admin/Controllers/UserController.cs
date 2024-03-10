using DocumentFormat.OpenXml.Vml;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.RoleDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.DTO.UserRoleDTO;
using ERPProject.Entity.Poco;
using ERPProject.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using System.Linq;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UserController(HttpClient httpClient, IWebHostEnvironment hostingEnvironment) : base(httpClient)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("/Admin/Kullanicilar")]
        public async Task<IActionResult> Index()
        {
            var dep = HttpContext.Session.GetString("DepartmentName");

            var admin = "Admin";




            var roles = SessionRole.RoleName;



            if ((!(roles.Any(roles => roles.Contains("Personel"))) || dep == "Bilgi İşlem") || dep == "İnsan Kaynakları")
            {
                var roller = HttpContext.Session.Keys.Any(x => x.Equals("RoleName0"));
                var id = HttpContext.Session.GetString("User");
                var val = await GetAllAsync<UserDTOResponse>(url + "GetUsersByDepartment/" + id);
                if (roles.Any(roles => roles.Contains("Admin") || dep == "Bilgi İşlem"))
                {
                    val = await GetAllAsync<UserDTOResponse>(url + "GetUsers");
                }
                if (roles.Any(roles => roles.Contains("Şirket Müdürü")) || (dep == "İnsan Kaynakları"))
                {
                    val = await GetAllAsync<UserDTOResponse>(url + "GetUsersByCompany/" + id);
                }
                if (roles.Any(roles => roles.Contains("Departman Müdürü")) && dep != "İnsan Kaynakları" && dep != "Bilgi İşlem")
                {
                    val = await GetAllAsync<UserDTOResponse>(url + "GetUsersByDepartment/" + id);
                }
                if (val.StatusCode == 401)
                {
                    return RedirectToAction("Unauthorized", "Home");
                }
                if (val.StatusCode == 403)
                {
                    return RedirectToAction("Forbidden", "Home");
                }

                var val2 = await GetAllAsync<DepartmentDTOResponse>(url + "GetDepartments");
                var val3 = await GetAllAsync<RoleDTOResponse>(url + "Roles");
                var val4 = await GetAllAsync<CompanyDTOResponse>(url + "GetCompanies");
                var val5 = await GetAllAsync<UserRoleDTOResponse>(url + "GetUserRoles");
                var userVM = new UserVM
                {
                    Departments = val2.Data,
                    Users = val.Data,
                    Roles = val3.Data,
                    Requests = null,
                    Companies = val4.Data,
                    UserRoles = val5.Data,

                };

                return View(userVM);



            }


            return RedirectToAction("Forbidden", "Home");
        }
        [HttpGet("/Admin/Kullanici")]
        public async Task<IActionResult> Profile()
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val = await GetAsync<UserDTOResponse>(url + "GetUser/" + userId);
            //var val1 = await GetAsync<UserRoleDTOResponse>(url + "GetUserRole/" + userId);
            if (val.StatusCode == 401)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else if (val.StatusCode == 403)
            {
                return RedirectToAction("Forbidden", "Home");
            }
            return View(val.Data);
        }
        [HttpPost("/Admin/KullaniciEkle")]
        public async Task<IActionResult> Add(UserDTORequest p, IFormFile imageFile, int RoleId)
        {
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (dep == "Insan Kaynaklari" || dep == "Yönetim" || dep == "Admin")
            {

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var imagePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "UserImage", uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    p.Image = "UserImage/" + uniqueFileName;
                    p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                    p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                    var val = await AddAsync(p, url + "AddUser");
                    UserRoleDTORequest y = new UserRoleDTORequest();
                    y.UserId = val.Data.Id;
                    y.RoleId = RoleId;
                    await AddAsync(y, url + "AddUserRole");
                    if (val.StatusCode == 401)
                    {
                        return RedirectToAction("Unauthorized", "Home");
                    }
                    else if (val.StatusCode == 403)
                    {
                        return RedirectToAction("Forbidden", "Home");
                    }
                    if (val == null)
                    {
                        return RedirectToAction("Forbidden", "Home");
                    }
                    if (val.Data != null)
                    {
                        return RedirectToAction("Index", "User");

                    }
                }
            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpPost("/Admin/KullaniciGuncelle")]
        public async Task<IActionResult> Update(UserDTORequest p, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var imagePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "UserImage", uniqueFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    System.IO.File.Delete("wwwroot/" + p.Image);
                    await imageFile.CopyToAsync(stream);

                }
                p.Image = "UserImage/" + uniqueFileName;
                p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                
                var val = await UpdateAsync(p, url + "UpdateUser");
                if (val.StatusCode == 200)
                {
                    return RedirectToAction("Kullanicilar", "Admin");

                }
            }
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val2 = await UpdateAsync(p, url + "UpdateUser");
            if (val2.StatusCode == 200 && val2.Data.Id.ToString() == HttpContext.Session.GetString("User"))
            {

                return RedirectToAction("Profile", "User");
            }
            else if (val2.StatusCode == 200 && val2.Data.Id.ToString() != HttpContext.Session.GetString("User"))
            {
                    return RedirectToAction("Kullanicilar", "Admin");

            }

            return RedirectToAction("Index", "Home");
        }
        [HttpGet("/Admin/KullaniciSil/{id}")]
        public async Task<IActionResult> Delete(Int64 id)
        {
            var val = await DeleteAsync(url + "RemoveUser/" + id);
            var dep = HttpContext.Session.GetString("DepartmentName");
            if (!(dep != "Insan Kaynaklari" || dep != "Yönetim" || dep != "Admin"))
            {
                return RedirectToAction("Forbidden", "Home");
            }
            if (val)
            {
                return RedirectToAction("Kullanicilar", "Admin");

            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpPost("/Admin/KullaniciRolEkle")]
        public async Task<IActionResult> UserRoleAdd(UserRoleDTORequest p)
        {
            var val = await AddAsync(p, url + "AddUserRole");
            if (val.Data != null)
            {
                return RedirectToAction("Kullanicilar", "Admin");

            }
            if (val.StatusCode==400)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            return RedirectToAction("Forbidden", "Home");

        }
        [HttpGet("/Admin/KullaniciRolSil")]
        public async Task<IActionResult> UserRoleDelete(UserRoleDTORequest p)
        {
            var val = await AddAsync(p, url + "RemoveUserRole");
            if (val.Data != null)
            {
                return RedirectToAction("Kullanicilar", "Admin");

            }
            return RedirectToAction("Kullanicilar", "Admin");

        }
    }
}
