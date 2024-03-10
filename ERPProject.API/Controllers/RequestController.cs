using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using System.Net.Mail;
using System.Text;

namespace ERPProject.API.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Authorize]

    public class RequestController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRequestService _requestService;
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly ICompanyService _companyService;


        public RequestController(IRequestService requestService, IMapper mapper, IUserService userService, IDepartmentService departmentService, ICompanyService companyService)
        {
            _userService = userService;
            _requestService = requestService;
            _mapper = mapper;
            _departmentService = departmentService;
            _companyService = companyService;
        }
        [HttpGet("/Requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests= await _requestService.GetAllAsync(x=>x.IsActive==true,"User","Product","AcceptedUser");
            
            if (requests==null)
            {
                return NotFound(Sonuc<List<RequestDTOResponse>>.SuccessNoDataFound());
            }

            List<RequestDTOResponse> requestDTOResponseList = new();

            foreach (var request in requests)
            {
                requestDTOResponseList.Add(_mapper.Map<RequestDTOResponse>(request));
            }

            Log.Information("Requests => {@requestDTOResponse} => { İstekler Getirildi. }", requestDTOResponseList);

            return Ok(Sonuc<List<RequestDTOResponse>>.SuccessWithData(requestDTOResponseList));

        }
        [HttpPost("/UpdateRequest")]
        [ValidationFilter(typeof(RequestValidator))]
        public async Task<IActionResult> UpdateRequest(RequestDTORequest requestDTORequest)
        {
            var request = await _requestService.GetAsync(e => e.Id == requestDTORequest.Id, "User", "Product");
            if (request == null)
            {
                return NotFound(Sonuc<RequestDTOResponse>.SuccessNoDataFound());
            }

            _mapper.Map(requestDTORequest, request);
            await _requestService.UpdateAsync(request);

            string AcceptRequestMessage = request.User.Name + " " + request.User.LastName + " adlı personelimiz " + request.Title + " başlıklı isteğiniz tamamlanmıştır.";
            string RefuseRequestMessage = request.User.Name + " " + request.User.LastName + " adlı personelimiz" + request.Title + "başlıklı isteğiniz reddeldilmiştir.";

            if (request.RequestStatus == 2)
            {
                SendMail(request.User.Email, AcceptRequestMessage);

            }
            if (request.RequestStatus == 3)
            {
                SendMail(request.User.Email, RefuseRequestMessage);
            }

            RequestDTOResponse requestDTOResponse = _mapper.Map<RequestDTOResponse>(request);

            Log.Information("Requests => {@requestDTOResponse} => { İstek Güncellendi. }", requestDTOResponse);

            return Ok(Sonuc<RequestDTOResponse>.SuccessWithData(requestDTOResponse));

        }

        [HttpDelete("/RemoveRequest/{requestId}")]
        public async Task<IActionResult> RemoveRequest(int requestId)
        {
            var request = await _requestService.GetAsync(e => e.Id == requestId);
            if (request == null)
            {
                return NotFound(Sonuc<RequestDTOResponse>.SuccessNoDataFound());
            }

            await _requestService.RemoveAsync(request);

            Log.Information("Requests => {@request} => { İstek Silindi. }", request);

            return Ok(Sonuc<RequestDTOResponse>.SuccessWithoutData());

        }

        [HttpPost("/AddRequest")]
        //[ValidationFilter(typeof(RequestValidator))]
        public async Task<IActionResult> AddRequest(RequestDTORequest requestDTORequest)
        {
            var request = _mapper.Map<Request>(requestDTORequest);

            await _requestService.AddAsync(request);
            RequestDTOResponse requestDTOResponse = _mapper.Map<RequestDTOResponse>(request);

            Log.Information("Requests => {@requestDTOResponse} => { İstek Eklendi. }", requestDTOResponse);
            return Ok(Sonuc<RequestDTOResponse>.SuccessWithData(requestDTOResponse));


        }

        [HttpGet("/Requests/{userId}")]
        public async Task<IActionResult> GetRequests(int userId)
        {
            var requests = await _requestService.GetAllAsync(e => e.UserId == userId && e.IsActive == true, "User", "Product");
            if (requests == null)
            {
                return NotFound(Sonuc<List<RequestDTOResponse>>.SuccessNoDataFound());
            }

            List<RequestDTOResponse> requestDTOResponseList = new();

            foreach (var request in requests)
            {
                requestDTOResponseList.Add(_mapper.Map<RequestDTOResponse>(request));
            }

            Log.Information("Requests => {@requestDTOResponse} => { Kullanıcıya Göre İstek Getirildi. }", requestDTOResponseList);
            return Ok(Sonuc<List<RequestDTOResponse>>.SuccessWithData(requestDTOResponseList));

        }
        [HttpGet("/Request/{requestId}")]
        public async Task<IActionResult> GetRequest(int requestId)
        {
            var request = await _requestService.GetAsync(e => e.Id == requestId);
            if (request == null)
            {
                return NotFound(Sonuc<RequestDTOResponse>.SuccessNoDataFound());
            }

            RequestDTOResponse requestDTOResponse = _mapper.Map<RequestDTOResponse>(request);


            Log.Information("Requests => {@requestDTOResponse} => { İstek Getirildi. } ", requestDTOResponse);

            return Ok(Sonuc<RequestDTOResponse>.SuccessWithData(requestDTOResponse));


        }


        [HttpGet("/RequestsByCompany/{userId}")]
        [Authorize(Roles = "Admin,Şirket Müdürü,Kullanıcı İşlemleri,Satın Alma İşlemleri")]
        public async Task<IActionResult> GetRequestsByCompany(long userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId);
            Department department = await _departmentService.GetAsync(x => x.Id == user.DepartmentId);
            Company company = await _companyService.GetAsync(x => x.Id == department.CompanyId);



            var requests = await _requestService.GetAllAsync(x => x.IsActive == true && x.User.Department.CompanyId == company.Id, "User", "Product");
            if (requests == null)
            {
                return NotFound(Sonuc<List<RequestDTOResponse>>.SuccessNoDataFound());
            }

            List<RequestDTOResponse> requestDTOResponseList = new();

            foreach (var request in requests)
            {
                requestDTOResponseList.Add(_mapper.Map<RequestDTOResponse>(request));
            }

            Log.Information("Requests => {@requestDTOResponse} => { İstekler Getirildi. }", requestDTOResponseList);

            return Ok(Sonuc<List<RequestDTOResponse>>.SuccessWithData(requestDTOResponseList));

        }
        [Authorize(Roles = "Admin,Departman Müdürü")]
        [HttpGet("/RequestsByDepartment/{userId}")]
        public async Task<IActionResult> GetRequestsByDepartment(long userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId);
            Department department = await _departmentService.GetAsync(x => x.Id == user.DepartmentId);

            var requests = await _requestService.GetAllAsync(x => x.IsActive == true && x.User.DepartmentId == department.Id, "User", "Product");

            if (requests == null)
            {
                return NotFound(Sonuc<List<RequestDTOResponse>>.SuccessNoDataFound());
            }

            List<RequestDTOResponse> requestDTOResponseList = new();

            foreach (var request in requests)
            {
                requestDTOResponseList.Add(_mapper.Map<RequestDTOResponse>(request));
            }

            Log.Information("Requests => {@requestDTOResponse} => { İstekler Getirildi. }", requestDTOResponseList);

            return Ok(Sonuc<List<RequestDTOResponse>>.SuccessWithData(requestDTOResponseList));

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
            a.Credentials = new System.Net.NetworkCredential("teklifbilgilendirme@hotmail.com", "Bilal123");
            a.Port = 587;
            a.Host = "smtp.office365.com";
            a.EnableSsl = true;
            object userState = mesaj;


            a.Send(mesaj);
        }
        [AllowAnonymous]
        [HttpGet("/RequestsByUser/{userId}")]
        public async Task<IActionResult> GetRequestsByUser(long userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId);

            var requests = await _requestService.GetAllAsync(x => x.IsActive == true && x.User.Id == userId, "User", "Product");

            if (requests == null)
            {
                return NotFound(Sonuc<List<RequestDTOResponse>>.SuccessNoDataFound());
            }

            List<RequestDTOResponse> requestDTOResponseList = new();

            foreach (var request in requests)
            {
                requestDTOResponseList.Add(_mapper.Map<RequestDTOResponse>(request));
            }

            Log.Information("Requests => {@requestDTOResponse} => { İstekler Kullanıcıya Göre Getirildi. }", requestDTOResponseList);

            return Ok(Sonuc<List<RequestDTOResponse>>.SuccessWithData(requestDTOResponseList));

        }


        //[HttpGet("/RequestsByDepartment/{userId}")]
        //public async Task<IActionResult> GetRequestsByDepartment(long userId)
        //{
        //    User user = await _userService.GetAsync(x => x.Id == userId);
        //    Department department = await _departmentService.GetAsync(x => x.Id == user.DepartmentId);

        //    var requests = await _requestService.GetAllAsync(x => x.IsActive == true && x.User.DepartmentId == department.Id, "User", "Product");

        //    if (requests == null)
        //    {
        //        return NotFound(Sonuc<List<RequestDTOResponse>>.SuccessNoDataFound());
        //    }

        //    List<RequestDTOResponse> requestDTOResponseList = new();

        //    foreach (var request in requests)
        //    {
        //        requestDTOResponseList.Add(_mapper.Map<RequestDTOResponse>(request));
        //    }

        //    Log.Information("Requests => {@requestDTOResponse} => { İstekler Getirildi. }", requestDTOResponseList);

        //    return Ok(Sonuc<List<RequestDTOResponse>>.SuccessWithData(requestDTOResponseList));

        //}

    }
}
