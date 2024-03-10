using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Entity.DTO.LoginDTO;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERPProject.API.Controllers
{
    [ApiController]
    [Route("[action]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public LoginController(IConfiguration configuration, IUserService userService, IMapper mapper, IUserRoleService userRoleService)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
            _userRoleService = userRoleService;
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> LoginAsync(LoginRequestDTO loginRequestDTO)
        {


            var user = await _userService.GetAsync(x => x.Email == loginRequestDTO.KullaniciAdi, "Department.Company", "UserRoles.Role");
            //var roles = await _userRoleService.GetAllAsync(x => x.User.Id==user.Id,"Role");
            if (user == null)
            {
                return Ok(Sonuc<LoginResponseDTO>.SuccessNoDataFound());
            }


            if (BCrypt.Net.BCrypt.Verify(loginRequestDTO.Sifre, user.Password))
            {
                List<Claim> claims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name , user.Name),
                new Claim(ClaimTypes.Surname , user.LastName),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim("EmailForMW", user.Email)
                };

                foreach (var item in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
                }

                var secretKey = _configuration["JWT:Token"];
                var issuer = _configuration["JWT:Issuer"];
                var audiance = _configuration["JWT:Audiance"];


                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = audiance,
                    Issuer = issuer,
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(20), // Token süresi (örn: 20 dakika)
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponseDTO loginResponseDTO = new()
                {
                    AdSoyad = user.Name + " " + user.LastName,
                    EPosta = user.Email,
                    Token = tokenHandler.WriteToken(token),
                    UserId = user.Id,
                    RoleName = user.UserRoles.Select(e => e.Role.Name).ToList(),
                    CompanyId = user.Department.CompanyId,
                    DepartmentId = user.DepartmentId,
                    DepartmentName = user.Department.Name,
                    CompanyName=user.Department.Company.Name

                };

                Log.Information("LoginResponse => {@loginResponseDTO} => { Giriş Yapıldı. }", loginResponseDTO);
                return Ok(Sonuc<LoginResponseDTO>.SuccessWithData(loginResponseDTO));
            }


            return Ok(Sonuc<LoginResponseDTO>.SuccessNoDataFound());

        }
       

    }
}
