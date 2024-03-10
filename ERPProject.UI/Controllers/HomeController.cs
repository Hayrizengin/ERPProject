using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Vml;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;
using ERPProject.UI.Areas;
using ERPProject.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;

namespace ERPProject.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly string url = "https://localhost:7075/";

        public HomeController(HttpClient httpClient) : base(httpClient)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Unauthorized()
        {
            return View();
        }
        public IActionResult BadRequest()
        {
            return View();
        }
        public IActionResult Forbidden()
        {
            return View();
        }
        [HttpGet("/SifreSifirla")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string mail)
        {
            var user = await GetAsync<UserDTORequest>(url + "GetUserByMail/" + mail);
            if (user == null)
            {
                return View("Error");
            }
            else
            {
                Random rnd = new Random();
                int password = rnd.Next(100000000, 999999999);
                UserDTORequest userDTORequest = new UserDTORequest
                {
                    Id = user.Data.Id,
                    DepartmentId = user.Data.DepartmentId,
                    Name = user.Data.Name,
                    LastName = user.Data.LastName,
                    Email = user.Data.Email,
                    Phone = user.Data.Phone,
                    Password = password.ToString(),//random oluştur
                    Image = user.Data.Image,
                    AddedUser = user.Data.AddedUser,
                    UpdatedUser = user.Data.UpdatedUser,

                };
               var response= await UpdateAsync<UserDTORequest>(userDTORequest, url + "UpdateUser/");
                if (response.StatusCode ==200)
                {
                    string AcceptRequestMessage = userDTORequest.Name + " " + userDTORequest.LastName + " adlı personelimiz Şifreniz:" + userDTORequest.Password + " olarak değiştirilmiştir.Hesabınıza giriş yaptıktan sonra lütfen şifrenizi değiştiriniz.";
                    SendMail(userDTORequest.Email, AcceptRequestMessage);
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
        }
        private void SendMail(string mail, string body)
        {

            MailMessage mesaj = new MailMessage();
            mesaj.From = new MailAddress("teklifbilgilendirme@hotmail.com");
            mesaj.To.Add(mail);
            mesaj.Subject = "İstek Sonuçlandı";
            mesaj.Body = body;
            mesaj.IsBodyHtml = true;
            mesaj.BodyEncoding = Encoding.UTF8;
            SmtpClient a = new SmtpClient();
            a.Credentials = new System.Net.NetworkCredential("teklifbilgilendirme@hotmail.com", "Hakanceylan123");
            a.Port = 587;
            a.Host = "smtp.office365.com";
            a.EnableSsl = true;
            object userState = mesaj;


            a.Send(mesaj);
        }
    }
}
