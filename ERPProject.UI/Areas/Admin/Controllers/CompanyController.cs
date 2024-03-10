using DocumentFormat.OpenXml.Vml;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.Poco;
using ERPProject.UI.Areas.Admin.Models;
using ERPProject.UI.Areas.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Net;

namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[AuthenticationFilter(Role = "Admin,Şirket Müdürü")]
    public class CompanyController : BaseController
    {
        private readonly string url = "https://localhost:7075/";
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CompanyController(HttpClient httpClient, IWebHostEnvironment hostingEnvironment) : base(httpClient)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("/Admin/Sirketler")]
        public async Task<IActionResult> Index()
        {
            var val = await GetAllAsync<CompanyDTOResponse>(url + "GetCompanies");

            foreach (var item in SessionRole.RoleName)
            {


                var id = HttpContext.Session.GetString("User");
                if (item=="Admin")
                {
                    val = await GetAllAsync<CompanyDTOResponse>(url + "GetCompanies");

                }
                else
                {
                    val = await GetAllAsync<CompanyDTOResponse>(url + "GetCompaniesByUser/" + id);

                }

                var val2 = await GetAllAsync<DepartmentDTOResponse>(url + "GetDepartments");
                if (val.StatusCode == 401)
                {
                    return RedirectToAction("Unauthorized", "Home");
                }
                else if (val.StatusCode == 403)
                {
                    return RedirectToAction("Forbidden", "Home");
                }


                CompanyVM companyVM = new CompanyVM()

                {
                    Companies = val.Data,
                    Departments = val2.Data,
                };
                return View(companyVM);
            }
            return RedirectToAction("Forbidden", "Home");

            
        }
        [HttpGet("/Admin/Sirket")]
        public async Task<IActionResult> Get(long id)
        {
            var val = await GetAsync<CompanyDTOResponse>(url + "GetCompany/" + id);

            return View(val);
        }
        [HttpPost("/Admin/SirketEkle")]
        public async Task<IActionResult> AddCompany(CompanyDTORequest p, IFormFile compImage)
        {
            if (compImage != null && compImage.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + compImage.FileName;
                var imagePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "CompanyImage", uniqueFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await compImage.CopyToAsync(stream);

                }
                p.Image = "CompanyImage/" + uniqueFileName;
                p.AddedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                var val = await AddAsync(p, url + "AddCompany");
                if (val.Data != null)
                {
                    return RedirectToAction("Index", "Company");

                }
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost("/Admin/SirketGuncelle")]
        public async Task<IActionResult> Update(CompanyDTORequest p, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var imagePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "CompanyImage", uniqueFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    System.IO.File.Delete("wwwroot/" + p.Image);
                    await imageFile.CopyToAsync(stream);

                }
                p.Image = "CompanyImage/" + uniqueFileName;
                p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
                var val = await UpdateAsync(p, url + "UpdateCompany");
                if (val.StatusCode == 200)
                {
                    return RedirectToAction("Sirketler", "Admin");

                }
            }
            p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            var val2 = await UpdateAsync(p, url + "UpdateCompany");
            if (val2.StatusCode == 200)
            {
                return RedirectToAction("Sirketler", "Admin");

            }

            return RedirectToAction("Index", "Home");
            //p.UpdatedUser = Convert.ToInt64(HttpContext.Session.GetString("User"));
            //var val = await UpdateAsync(p, url + "UpdateCompany");
            //if (val.StatusCode == 200)
            //{
            //    return RedirectToAction("Sirketler", "Admin");

            //}

            //return RedirectToAction("Index", "Home");

        }
        [HttpGet("/Admin/SirketSil/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var val = await DeleteAsync(url + "RemoveCompany/" + id);
            if (val)
            {
                return RedirectToAction("Index", "Company");
            }
            return RedirectToAction("Index", "Home");

        }

    }
}
