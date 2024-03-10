using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.DepartmentDTO;
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
    
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IMapper mapper, IDepartmentService departmentService, IUserService userService, ICompanyService companyService)
        {
            _mapper = mapper;
            _departmentService = departmentService;
        }
        [HttpPost("/AddDepartment")]
        [ValidationFilter(typeof(DepartmentValidator))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment(DepartmentDTORequest departmentDTORequest)
        {
            Department department = _mapper.Map<Department>(departmentDTORequest);
            await _departmentService.AddAsync(department);

            DepartmentDTOResponse departmentDTOResponse = _mapper.Map<DepartmentDTOResponse>(department);

            Log.Information("Departments => {@departmentDTOResponse} => { Departman Eklendi. }", departmentDTOResponse);

            return Ok(Sonuc<DepartmentDTOResponse>.SuccessWithData(departmentDTOResponse));
        }
        [HttpDelete("/RemoveDepartment/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveDepartment(int departmentId)
        {
            Department department = await _departmentService.GetAsync(x=>x.Id == departmentId);

            if (department == null)
            {
                return NotFound(Sonuc<DepartmentDTOResponse>.SuccessNoDataFound());
            }
            
            await _departmentService.RemoveAsync(department);

            Log.Information("Departments => {@department} => { Departman Silindi. }", department);

            return Ok(Sonuc<DepartmentDTOResponse>.SuccessWithoutData());
        }
        [HttpPost("/UpdateDepartment")]
        [ValidationFilter(typeof(DepartmentValidator))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(DepartmentDTORequest departmentDTORequest)
        {
            Department department = await _departmentService.GetAsync(x=>x.Id == departmentDTORequest.Id);
            if (department == null)
            {
                return NotFound(Sonuc<DepartmentDTOResponse>.SuccessNoDataFound());
            }
            _mapper.Map(departmentDTORequest,department);

            var existingDepartment = await _departmentService.GetAsync(x => x.Name == department.Name);

            if (existingDepartment != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu departman zaten var"));
            }

            await _departmentService.UpdateAsync(department);


            DepartmentDTOResponse departmentDTOResponse = _mapper.Map<DepartmentDTOResponse>(department);

            Log.Information("Departments => {@departmentDTOResponse} => { Departman Güncellendi. }", departmentDTOResponse);

            return Ok(Sonuc<DepartmentDTOResponse>.SuccessWithData(departmentDTOResponse));
        }


        [HttpGet("/GetDepartment/{departmentId}")]
        public async Task<IActionResult> GetDepartment(int departmentId)
        {
            Department department = await _departmentService.GetAsync(x => x.Id == departmentId, "Company");

            if (department == null)
            {
                return NotFound(Sonuc<DepartmentDTOResponse>.SuccessNoDataFound());
            }
            DepartmentDTOResponse departmentDTOResponse = _mapper.Map<DepartmentDTOResponse>(department);

            Log.Information("Departments => {@departmentDTOResponse} => { Department Getirildi. }", departmentDTOResponse);

            return Ok(Sonuc<DepartmentDTOResponse>.SuccessWithData(departmentDTOResponse));
        }

        [HttpGet("/GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetAllAsync(x=>x.IsActive==true,"Company");
            if (departments == null)
            {
                return NotFound(Sonuc<DepartmentDTOResponse>.SuccessNoDataFound());
            }

            List<DepartmentDTOResponse> departmentDTOResponseList = new();
            foreach (var department in departments)
            {
                departmentDTOResponseList.Add(_mapper.Map<DepartmentDTOResponse>(department));
            }

            Log.Information("Departments => {@departmentDTOResponse} => { Departmanlar Getirildi. }", departmentDTOResponseList);

            return Ok(Sonuc<List<DepartmentDTOResponse>>.SuccessWithData(departmentDTOResponseList));
        }


        [HttpGet("/GetDepartmentsByCompany/{companyId}")]
        public async Task<IActionResult> GetDepartmentsByCompany(int companyId)
        {
            var departments = await _departmentService.GetAllAsync(x => x.IsActive == true && x.CompanyId == companyId, "Company");

            if (departments == null)
            {
                return NotFound(Sonuc<DepartmentDTOResponse>.SuccessNoDataFound());
            }

            List<DepartmentDTOResponse> departmentDTOResponseList = new();
            foreach (var department in departments)
            {
                departmentDTOResponseList.Add(_mapper.Map<DepartmentDTOResponse>(department));
            }

            Log.Information("Departments => {@departmentDTOResponse} => { Şirketlere Göre Departmanlar Getirildi. }", departmentDTOResponseList);

            return Ok(departmentDTOResponseList);
        }

        
    }
    
}
