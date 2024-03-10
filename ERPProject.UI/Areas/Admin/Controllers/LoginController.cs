using ERPProject.Entity.DTO.UserLoginDTO;
using ERPProject.Entity.Result;
using ERPProject.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;


namespace ERPProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        //private readonly string url = "https://localhost:7075/";
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        [HttpPost("/Admin/GirisYap")]
        public async Task<IActionResult> Index(LoginDTO p)
        {
            var url = "https://localhost:7075/Login";
            var client = new RestClient(url);
            var request = new RestRequest(url, RestSharp.Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(p);
            request.AddBody(body, "application/json");
            RestResponse response = await client.ExecuteAsync(request);
            var responseObject = JsonConvert.DeserializeObject<ApiResponse<LoginDTO>>(response.Content);
            
            if (responseObject.StatusCode == 200)
            {
                HttpContext.Session.SetString("Token", responseObject.Data.Token);
                HttpContext.Session.SetString("Company", responseObject.Data.CompanyId.ToString());
                HttpContext.Session.SetString("Department", responseObject.Data.DepartmentId.ToString());
                HttpContext.Session.SetString("CompanyName", responseObject.Data.CompanyName);
                HttpContext.Session.SetString("DepartmentName", responseObject.Data.DepartmentName);
                HttpContext.Session.SetString("UserName", responseObject.Data.AdSoyad.ToString());
                HttpContext.Session.SetString("User", responseObject.Data.UserId.ToString());

                //for (int i = 0; i < responseObject.Data.RoleName.Count; i++)
                //{
                //    HttpContext.Session.SetString("RoleName" + i, responseObject.Data.RoleName[i]);
                //}
                
                
                    SessionRole.RoleName=responseObject.Data.RoleName;
                

            }
            else
            {
                TempData["HATA"] = "Hatalı Giriş Yaptınız";
                return RedirectToAction("", "");
            }
            HttpContext.Session.SetString("User", responseObject.Data.UserId.ToString());


            if (responseObject!=null)
            {
                return RedirectToAction("Index", "UserHome");
            }
            


            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/Logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            SessionRole.RoleName.Clear();
            return RedirectToAction("", "");
        }
    }
}
