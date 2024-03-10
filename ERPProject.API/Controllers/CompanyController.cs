using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace ERPProject.API.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CompanyController(IMapper mapper, ICompanyService companyService, IDepartmentService departmentService, IUserService userService)
        {
            _mapper = mapper;
            _companyService = companyService;
            _departmentService = departmentService;
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/AddCompany")]
        [ValidationFilter(typeof(CompanyValidator))]
        //[Authorize(Roles = "Admin,Yönetim Kurulu Başkanı,Şirket Müdürü")]
        public async Task<ActionResult> AddCompany(CompanyDTORequest companyDTORequest)
        {
            Company company = _mapper.Map<Company>(companyDTORequest);

            var existingCompany = await _companyService.GetAsync(x => x.Name == company.Name);

            if (existingCompany != null)
            {
                return BadRequest(Sonuc<CompanyDTOResponse>.ExistingError("Bu şirket zaten var"));
            }

            await _companyService.AddAsync(company);


            CompanyDTOResponse companyDTOResponse = _mapper.Map<CompanyDTOResponse>(company);

            Log.Information("Companies => {@companyDTOResponse} => { Şirket Eklendi. }", companyDTOResponse);

            return Ok(Sonuc<CompanyDTOResponse>.SuccessWithData(companyDTOResponse));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/RemoveCompany/{id}")]
        public async Task<IActionResult> RemoveCompany(int id)
        {
            Company company = await _companyService.GetAsync(x => x.Id == id);

            if (company == null)
            {
                return NotFound(Sonuc<CompanyDTOResponse>.SuccessNoDataFound());
            }

            await _companyService.RemoveAsync(company);

            Log.Information("Companies => {@company} => { Şirket Silindi. }", company);

            return Ok(Sonuc<CompanyDTOResponse>.SuccessWithoutData());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/UpdateCompany")]
        [ValidationFilter(typeof(CompanyValidator))]
        public async Task<IActionResult> UpdateCompany(CompanyDTORequest companyDTORequest)
        {
            Company company = await _companyService.GetAsync(x => x.Id == companyDTORequest.Id);
            var companylist = await _companyService.GetAllAsync();
            if (company == null)
            {
                return NotFound(Sonuc<CompanyDTORequest>.SuccessNoDataFound());
            }
            else if (company.Name == companyDTORequest.Name)
            {
                company = _mapper.Map(companyDTORequest, company);
                await _companyService.UpdateAsync(company);

                CompanyDTOResponse companyDTOResponse2 = _mapper.Map<CompanyDTOResponse>(company);

                Log.Information("Companies => {@companyDTOResponse} => { Kullanıcı Güncellendi. }", companyDTOResponse2);

                return Ok(Sonuc<CompanyDTOResponse>.SuccessWithData(companyDTOResponse2));
            }

            else if (companylist.Any(x => x.Name == companyDTORequest.Name))
            {
                return BadRequest(Sonuc<CompanyDTOResponse>.ExistingError("Bu şirket zaten var"));
            }
            company = _mapper.Map(companyDTORequest, company);
            await _companyService.UpdateAsync(company);

            CompanyDTOResponse companyDTOResponse = _mapper.Map<CompanyDTOResponse>(company);

            Log.Information("Companies => {@companyDTOResponse} => { Şirket Güncellendi. }", companyDTOResponse);

            return Ok(Sonuc<CompanyDTOResponse>.SuccessWithData(companyDTOResponse));
        }
        [AllowAnonymous]
        [HttpGet("/GetCompany/{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            Company company = await _companyService.GetAsync(x => x.Id == id);
            if (company == null)
            {
                return NotFound(Sonuc<CompanyDTOResponse>.SuccessNoDataFound());
            }

            CompanyDTOResponse companyDTOResponse = _mapper.Map<CompanyDTOResponse>(company);

            Log.Information("Companies => {@companyDTOResponse} => { Şirket Getirildi. }", companyDTOResponse);

            return Ok(Sonuc<CompanyDTOResponse>.SuccessWithData(companyDTOResponse));
        }
        [AllowAnonymous]
        [HttpGet("/GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyService.GetAllAsync(x => x.IsActive == true);
            if (companies == null)
            {
                return Ok(Sonuc<CompanyDTOResponse>.SuccessWithoutData());
            }
            List<CompanyDTOResponse> companyDTOResponseList = new();
            foreach (var company in companies)
            {
                companyDTOResponseList.Add(_mapper.Map<CompanyDTOResponse>(company));
            }

            Log.Information("Companies => {@companyDTOResponse} => { Şirketler Getirildi. } ", companyDTOResponseList);

            return Ok(Sonuc<List<CompanyDTOResponse>>.SuccessWithData(companyDTOResponseList));
        }
        [AllowAnonymous]
        [HttpGet("/GetCompaniesByUser/{userId}")]
        public async Task<IActionResult> GetCompaniesByUser(long userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId);
            Department department = await _departmentService.GetAsync(x => x.Id == user.DepartmentId);
            Company company = await _companyService.GetAsync(x => x.Id == department.CompanyId);

            var companies = await _companyService.GetAllAsync(x => x.IsActive == true && x.Id == company.Id);

            if (companies == null)
            {
                return Ok(Sonuc<CompanyDTOResponse>.SuccessWithoutData());
            }
            List<CompanyDTOResponse> companyDTOResponseList = new();
            foreach (var item in companies)
            {
                companyDTOResponseList.Add(_mapper.Map<CompanyDTOResponse>(item));
            }

            Log.Information("Companies => {@companyDTOResponse} => { Şirketler Getirildi. } ", companyDTOResponseList);

            return Ok(Sonuc<List<CompanyDTOResponse>>.SuccessWithData(companyDTOResponseList));
        }
    }
}
