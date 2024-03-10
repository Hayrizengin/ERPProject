using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ERPProject.API.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Authorize]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly IUserRoleService _userRoleService;

        public UserController(IMapper mapper, IUserService userService, ICompanyService companyService, IDepartmentService departmentService, IUserRoleService userRoleService)
        {
            _mapper = mapper;
            _userService = userService;
            _companyService = companyService;
            _departmentService = departmentService;
            _userRoleService = userRoleService;
        }
        //[Authorize(Roles = "Admin,Kullanıcı İşlemleri")]
        [HttpPost("/AddUser")]
        [ValidationFilter(typeof(UserValidator))]
        public async Task<IActionResult> AddUser(UserDTORequest userDTORequest)
        {
            User user = _mapper.Map<User>(userDTORequest);

            var existingUser = await _userService.GetAsync(x => x.Email == user.Email);

            if (existingUser != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu kullanıcı zaten var"));
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _userService.AddAsync(user);

            UserDTOResponse userDTOResponse = _mapper.Map<UserDTOResponse>(user);

            Log.Information("Users => {@userDTOResponse} => { Kullanıcı Eklendi. }", userDTOResponse);

            return Ok(Sonuc<UserDTOResponse>.SuccessWithData(userDTOResponse));
        }
        [Authorize(Roles = "Admin,Kullanıcı İşlemleri")]
        [HttpDelete("/RemoveUser/{userId}")]
        public async Task<IActionResult> RemoveUser(Int64 userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId, "UserRoles.Role");
            if (user == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }

            var userRole = await _userRoleService.GetAllAsync(x => x.User.Id == user.Id);

            foreach (var role in userRole.ToList())
            {
                await _userRoleService.RemoveAsync(role);
            }

            await _userService.RemoveAsync(user);

            Log.Information("Users => {@user} => { Kullanıcı Silindi. }", user);

            return Ok(Sonuc<UserDTOResponse>.SuccessWithoutData());
        }

        [HttpPost("/UpdateUser")]
        [AllowAnonymous]
        [ValidationFilter(typeof(UserValidator))]
        public async Task<IActionResult> UpdateUser(UserDTORequest userDTORequest)
        {
            User user = await _userService.GetAsync(x => x.Id == userDTORequest.Id); // güncellenecek kişi
            var userlist = await _userService.GetAllAsync(); // güncellenecek kişi
            if (user == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }
            else if (user.Email == userDTORequest.Email)
            {
                user = _mapper.Map(userDTORequest, user);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userDTORequest.Password);
                await _userService.UpdateAsync(user);

                UserDTOResponse userDTOResponse2 = _mapper.Map<UserDTOResponse>(user);

                Log.Information("Users => {@userDTOResponse} => { Kullanıcı Güncellendi. }", userDTOResponse2);

                return Ok(Sonuc<UserDTOResponse>.SuccessWithData(userDTOResponse2));
            }

            else if (userlist.Any(x => x.Email == userDTORequest.Email))
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu kullanıcı zaten var"));
            }

            user = _mapper.Map(userDTORequest, user);
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDTORequest.Password);
            await _userService.UpdateAsync(user);

            UserDTOResponse userDTOResponse = _mapper.Map<UserDTOResponse>(user);

            Log.Information("Users => {@userDTOResponse} => { Kullanıcı Güncellendi. }", userDTOResponse);

            return Ok(Sonuc<UserDTOResponse>.SuccessWithData(userDTOResponse));
        }
        [AllowAnonymous]
        [HttpGet("/GetUser/{userId}")]
        public async Task<IActionResult> GetUser(long userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId && x.IsActive == true, "UserRoles.Role", "Department", "Department.Company");
            if (user == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }

            UserDTOResponse userDTOResponse = _mapper.Map<UserDTOResponse>(user);

            Log.Information("Users => {@userDTOResponse} => { Kullanıcı Getirildi. }", userDTOResponse);

            return Ok(Sonuc<UserDTOResponse>.SuccessWithData(userDTOResponse));
        }
        [AllowAnonymous]
        [HttpGet("/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {

            var users = await _userService.GetAllAsync(x => x.IsActive == true, "Department.Company", "UserRoles.Role");

            if (users == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }
            List<UserDTOResponse> userDTOResponseList = new();
            foreach (var item in users)
            {
                userDTOResponseList.Add(_mapper.Map<UserDTOResponse>(item));

            }


            Log.Information("Users => {@userDTOResponse} => { Kullanıcılar Getirildi. }", userDTOResponseList);

            return Ok(Sonuc<List<UserDTOResponse>>.SuccessWithData(userDTOResponseList));
        }

        [Authorize(Roles = "Departman Müdürü")]
        [HttpGet("/GetUsersByDepartment/{userId}")]
        public async Task<IActionResult> GetUsersByDepartment(int userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId);
            Department department = await _departmentService.GetAsync(x => x.Id == user.DepartmentId);

            var users = await _userService.GetAllAsync(x => x.IsActive == true && x.DepartmentId == department.Id, "Department.Company", "UserRoles.Role");
            if (users == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }
            List<UserDTOResponse> userDTOResponseList = new();
            foreach (var item in users)
            {
                userDTOResponseList.Add(_mapper.Map<UserDTOResponse>(item));
            }

            Log.Information("Users => {@userDTOResponse} => { Departmana Göre Kullanıcılar Getirildi. }", userDTOResponseList);

            return Ok(Sonuc<List<UserDTOResponse>>.SuccessWithData(userDTOResponseList));
        }
        [HttpGet("/GetUsersByRole/{roleId}")]
        public async Task<IActionResult> GetUsersByRole(int roleId)
        {
            var users = await _userService.GetAllAsync(x => x.IsActive == true && x.DepartmentId == roleId, "Department.Company", "UserRoles.Role");
            if (users == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }
            List<UserDTOResponse> userDTOResponseList = new();
            foreach (var user in users)
            {
                userDTOResponseList.Add(_mapper.Map<UserDTOResponse>(user));
            }

            Log.Information("Users => {@userDTOResponse} => { Rollere Göre Kullanıcılar Getirildi. }", userDTOResponseList);

            return Ok(Sonuc<List<UserDTOResponse>>.SuccessWithData(userDTOResponseList));
        }
        [Authorize(Roles = "Kullanıcı İşlemleri,Personel")]
        [HttpGet("/GetUsersByCompany/{userId}")]
        public async Task<IActionResult> GetUserGetUsersByCompany(int userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId);
            Department department = await _departmentService.GetAsync(x => x.Id == user.DepartmentId);
            Company company = await _companyService.GetAsync(x => x.Id == department.CompanyId);


            var users = await _userService.GetAllAsync(x => x.IsActive == true && x.Department.CompanyId == company.Id, "Department.Company", "UserRoles.Role");
            if (users == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }
            List<UserDTOResponse> userDTOResponseList = new();
            foreach (var item in users)
            {

                userDTOResponseList.Add(_mapper.Map<UserDTOResponse>(item));

            }

            foreach (var item in userDTOResponseList)
            {
                item.CompanyName = company.Name;
            }

            Log.Information("Users => {@userDTOResponse} => { Şirkete Göre Kullanıcılar Getirildi. }", userDTOResponseList);

            return Ok(Sonuc<List<UserDTOResponse>>.SuccessWithData(userDTOResponseList));
        }
        [AllowAnonymous]
        [HttpGet("/GetUserByMail/{mail}")]

        public async Task<IActionResult> GetUserByMail(string mail)
        {
            User user = await _userService.GetAsync(x => x.Email == mail && x.IsActive == true, "UserRoles.Role", "Department", "Department.Company");
            if (user == null)
            {
                return NotFound(Sonuc<UserDTOResponse>.SuccessNoDataFound());
            }

            UserDTOResponse userDTOResponse = _mapper.Map<UserDTOResponse>(user);

            Log.Information("Users => {@userDTOResponse} => { Kullanıcı Getirildi. }", userDTOResponse);

            return Ok(Sonuc<UserDTOResponse>.SuccessWithData(userDTOResponse));
        }
    }


}
