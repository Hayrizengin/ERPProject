using AutoMapper;
using ERPProject.API.Mapper.UserRoleMap;
using ERPProject.Business.Abstract;
using ERPProject.Entity.DTO.UserRoleDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ERPProject.API.Controllers
{
    [ApiController]
    [Route("[Action]")]
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserRoleController(IMapper mapper, IUserRoleService userRoleService, IUserService userService)
        {
            _mapper = mapper;
            _userRoleService = userRoleService;
            _userService = userService;
        }

        [HttpPost("/AddUserRole")]
        public async Task<IActionResult> AddUserRole(UserRoleDTORequest userRoleDTORequest)
        {
            User user = await _userService.GetAsync(x=>x.Id == userRoleDTORequest.UserId);
            var userRoles = await _userRoleService.GetAsync(x=>x.UserId == user.Id && x.RoleId==userRoleDTORequest.RoleId);
            if (userRoles != null)
            {
                return BadRequest(Sonuc<UserRoleDTOResponse>.AlreadyExistError());
            }

            var userRole = _mapper.Map<UserRole>(userRoleDTORequest);
            await _userRoleService.AddAsync(userRole);

            UserRoleDTOResponse userRoleDTOResponse = _mapper.Map<UserRoleDTOResponse>(userRole);

            return Ok(Sonuc<UserRoleDTOResponse>.SuccessWithData(userRoleDTOResponse));
        }

        [HttpPost("/RemoveUserRole")]
        public async Task<IActionResult> DeleteUserRole(UserRoleDTORequest userRoleDTORequest)
        {
            var userRole = await _userRoleService.GetAsync(e => e.UserId == userRoleDTORequest.UserId && e.RoleId == userRoleDTORequest.RoleId);

            await _userRoleService.RemoveAsync(userRole);

            Log.Information("UserRole => {@userRole} => { Kullanıcıya Ait Rol Silindi. }", userRole);


            return Ok(Sonuc<UserRoleDTOResponse>.SuccessWithoutData());
        }

        [HttpGet("/GetUserRole/{userId}")]
        public async Task<IActionResult> GetUserRoleByUserId(UserRoleDTORequest userRoleDTORequest)
        {
            var userRoles = await _userRoleService.GetAllAsync(e => e.UserId == userRoleDTORequest.UserId );

            List<UserRoleDTOResponse> userRoleDTOResponses = new();


            foreach (var item in userRoles)
            {
                userRoleDTOResponses.Add(_mapper.Map<UserRoleDTOResponse>(item));
            }

            return Ok(Sonuc<List<UserRoleDTOResponse>>.SuccessWithData(userRoleDTOResponses));
        }
        [HttpGet("/GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var userRoles = await _userRoleService.GetAllAsync();

            List<UserRoleDTOResponse> userRoleDTOResponses = new();


            foreach (var item in userRoles)
            {
                userRoleDTOResponses.Add(_mapper.Map<UserRoleDTOResponse>(item));
            }

            return Ok(Sonuc<List<UserRoleDTOResponse>>.SuccessWithData(userRoleDTOResponses));
        }
        [HttpGet("/GetUserRolesJs/{id}")]// UserId ye göre Rolleri Getirecez burda 
        public async Task<IActionResult> GetUserRolesJs(int id)
        {
            var userRoles = await _userRoleService.GetAllAsync(x=> x.UserId == id, "Role");

            List<UserRoleDTOResponse> userRoleDTOResponses = new();


            foreach (var item in userRoles)
            {
                userRoleDTOResponses.Add(_mapper.Map<UserRoleDTOResponse>(item));
            }

            return Ok(userRoleDTOResponses);
        }
    }
}
