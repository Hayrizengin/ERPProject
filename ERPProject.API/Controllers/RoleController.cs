using AutoMapper;
using Azure.Core;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.RequestDTO;
using ERPProject.Entity.DTO.RoleDTO;
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
    [Authorize(Roles = "Admin,Kullanıcı İşlemleri")]

    public class RoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }


        [HttpGet("/Roles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllAsync(x => x.IsActive == true);

            if (roles == null)
            {
                return NotFound(Sonuc<List<RoleDTOResponse>>.SuccessNoDataFound());
            }
            List<RoleDTOResponse> roleDTOResponses = new();

            foreach (var item in roles)
            {
                roleDTOResponses.Add(_mapper.Map<RoleDTOResponse>(item));
            }

            Log.Information("Roles => {@roleDTOResponse} => { Roller Getirildi. }", roleDTOResponses);
            return Ok(Sonuc<List<RoleDTOResponse>>.SuccessWithData(roleDTOResponses));

        }
        [AllowAnonymous]
        [HttpGet("/Role/{roleId}")]
        public async Task<IActionResult> GetRole(int roleId)
        {
            var role =  await _roleService.GetAsync(e => e.Id == roleId);
            if(role == null)
            {
                return NotFound(Sonuc<RoleDTOResponse>.SuccessNoDataFound());
            }
            RoleDTOResponse roleDTOResponse = _mapper.Map<RoleDTOResponse>(role);

            Log.Information("Roles => {@roleDTOResponse} => { Rol Getirildi. }", roleDTOResponse);

            return Ok(Sonuc<RoleDTOResponse>.SuccessWithData(roleDTOResponse));

        }

        [HttpPost("/AddRole")]
        [ValidationFilter(typeof(RoleValidator))]
        public async Task<IActionResult> AddRole(RoleDTORequest roleDTORequest)
        {

            var role = _mapper.Map<Role>(roleDTORequest);

            var existingRole = await _roleService.GetAsync(x => x.Name == role.Name);

            if (existingRole != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu rol zaten var"));
            }

            await _roleService.AddAsync(role);
            RoleDTOResponse roleDTOResponse = _mapper.Map<RoleDTOResponse>(role);

            Log.Information("Roles => {@roleDTOResponse} => { Rol Eklendi. }", roleDTOResponse);

            return Ok(Sonuc<RoleDTOResponse>.SuccessWithData(roleDTOResponse));

        }
        [HttpPost("/UpdateRole")]
        [ValidationFilter(typeof(RoleValidator))]
        public async Task<IActionResult> UpdateRole(RoleDTORequest roleDTORequest)
        {
            Role role = await _roleService.GetAsync(e => e.Id == roleDTORequest.Id);

            if (role == null)
            {
                return NotFound(Sonuc<RoleDTOResponse>.SuccessNoDataFound());
            }

            _mapper.Map(roleDTORequest,role);

            var existingRole = await _roleService.GetAsync(x => x.Name == role.Name);

            if (existingRole != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu rol zaten var"));
            }

            await _roleService.UpdateAsync(role);

            RoleDTOResponse roleDTOResponse = _mapper.Map<RoleDTOResponse>(role);

            Log.Information("Roles => {@roleDTOResponse} => { Rol Güncellendi. }", roleDTOResponse);

            return Ok(Sonuc<RoleDTOResponse>.SuccessWithData(roleDTOResponse));
        }
        [HttpDelete("/RemoveRole/{roleId}")]
        public async Task<IActionResult> RemoveRole(int roleId)
        {
            Role role = await _roleService.GetAsync(e => e.Id == roleId);
            if (role == null)
            {
                return NotFound(Sonuc<RoleDTOResponse>.SuccessNoDataFound());
            }
            await _roleService.RemoveAsync(role);

            Log.Information("Roles => {@role} => { Rol Silindi. }", role);

            return Ok(Sonuc<RoleDTOResponse>.SuccessWithoutData());

        }



    }
}
