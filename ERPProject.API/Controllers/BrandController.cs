using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ERPProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Satın Alma İşlemleri")]
    public class BrandController : ControllerBase
    {
        
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandController(IMapper mapper, IBrandService brandService)
        {
            _mapper = mapper;
            _brandService = brandService;
        }
        [HttpPost("/AddBrand")]
        [ValidationFilter(typeof(BrandValidator))]
        public async Task<IActionResult> AddBrand(BrandDTORequest brandDTORequest)
        {
            Brand brand = _mapper.Map<Brand>(brandDTORequest);

            var existingBrand = await _brandService.GetAsync(x => x.Name == brand.Name);

            if (existingBrand != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu marka zaten var"));
            }

            await _brandService.AddAsync(brand);

            BrandDTOResponse brandDTOResponse = _mapper.Map<BrandDTOResponse>(brand);

            Log.Information("Brands => {@brandDTOResponse} => { Marka Eklendi. }", brandDTOResponse);

            return Ok(Sonuc<BrandDTOResponse>.SuccessWithData(brandDTOResponse));


        }
        [HttpDelete("/RemoveBrand/{brandId}")]
        public async Task<IActionResult> RemoveBrand(int brandId)
        {
            Brand brand = await _brandService.GetAsync(x => x.Id == brandId);
            if (brand == null)
            {
                return NotFound(Sonuc<BrandDTOResponse>.SuccessNoDataFound());
            }

            await _brandService.RemoveAsync(brand);

            Log.Information("Brands => {@brand} => { Marka Silindi. }", brand);

            return Ok(Sonuc<BrandDTOResponse>.SuccessWithoutData());
        }
        [HttpPost("/UpdateBrand")]
        [ValidationFilter(typeof(BrandValidator))]
        public async Task<IActionResult> UpdateBrand(BrandDTORequest brandDTORequest)
        {
            Brand brand = await _brandService.GetAsync(x => x.Id == brandDTORequest.Id);
            if (brand == null)
            {
                return NotFound(Sonuc<BrandDTOResponse>.SuccessNoDataFound());
            }
            brand = _mapper.Map(brandDTORequest, brand);

            var existingBrand = await _brandService.GetAsync(x => x.Name == brand.Name);

            if (existingBrand != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu marka zaten var"));
            }

            await _brandService.UpdateAsync(brand);

            BrandDTOResponse brandDTOResponse = _mapper.Map<BrandDTOResponse>(brand);

            Log.Information("Brands => {@brandDTOResponse} => { Marka Güncellendi. }", brandDTOResponse);

            return Ok(Sonuc<BrandDTOResponse>.SuccessWithData(brandDTOResponse));
        }
        [HttpGet("/GetBrand/{brandId}")]
        public async Task<IActionResult> GetBrand(int brandId)
        {
            Brand brand = await _brandService.GetAsync(x => x.Id == brandId );
            if (brand == null)
            {
                return NotFound(Sonuc<BrandDTOResponse>.SuccessNoDataFound());
            }

            BrandDTOResponse brandDTOResponse = _mapper.Map<BrandDTOResponse>(brand);

            Log.Information("Brands => {@brandDTOResponse} => { Marka Getirildi. }", brandDTOResponse);

            return Ok(Sonuc<BrandDTOResponse>.SuccessWithData(brandDTOResponse));
        }
        [HttpGet("/GetBrands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _brandService.GetAllAsync(x=>x.IsActive==true);
            if (brands == null)
            {
                return NotFound(Sonuc<BrandDTOResponse>.SuccessNoDataFound());
            }
            List<BrandDTOResponse> brandDTOResponseList = new();
            foreach (var brand in brands)
            {
                brandDTOResponseList.Add(_mapper.Map<BrandDTOResponse>(brand));
            }

            Log.Information("Brands => {@brandDTOResponse} => { Markalar Getirildi. }", brandDTOResponseList);

            return Ok(Sonuc<List<BrandDTOResponse>>.SuccessWithData(brandDTOResponseList));
        }
    }
}
