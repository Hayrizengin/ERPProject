using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.BrandDTO;
using ERPProject.Entity.DTO.CategoryDTO;
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

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }
        [HttpPost("/AddCategory")]
        [ValidationFilter(typeof(CategoryValidator))]
        public async Task<IActionResult> AddCategory(CategoryDTORequest categoryDTORequest)
        {
            Category category = _mapper.Map<Category>(categoryDTORequest);

            var existingCategory = await _categoryService.GetAsync(x => x.Name == category.Name);

            if (existingCategory != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu kategori zaten var"));
            }

            await _categoryService.AddAsync(category);

            CategoryDTOResponse categoryDTOResponse = _mapper.Map<CategoryDTOResponse>(category);

            Log.Information("Categories => {@categoryDTOResponse} => { Kategori Eklendi. }", categoryDTOResponse);

            return Ok(Sonuc<CategoryDTOResponse>.SuccessWithData(categoryDTOResponse));
        }
        [HttpDelete("/RemoveCategory/{categoryId}")]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            Category category = await _categoryService.GetAsync(x => x.Id == categoryId);
            if (category == null)
            {
                return NotFound(Sonuc<CategoryDTOResponse>.SuccessNoDataFound());
            }

            await _categoryService.RemoveAsync(category);

            Log.Information("Categories => {@category} => { Kategori Silindi. }", category);

            return Ok(Sonuc<CategoryDTOResponse>.SuccessWithoutData());
        }
        [HttpPost("/UpdateCategory")]
        [ValidationFilter(typeof(CategoryValidator))]
        public async Task<IActionResult> UpdateCategory(CategoryDTORequest categoryDTORequest)
        {
            Category category = await _categoryService.GetAsync(x => x.Id == categoryDTORequest.Id);
            if (category == null)
            {
                return NotFound(Sonuc<CategoryDTOResponse>.SuccessNoDataFound());
            }
            category = _mapper.Map(categoryDTORequest, category);

            var existingCategory = await _categoryService.GetAsync(x => x.Name == category.Name);

            if (existingCategory != null)
            {
                return BadRequest(Sonuc<UserDTOResponse>.ExistingError("Bu kategori zaten var"));
            }

            await _categoryService.UpdateAsync(category);

            CategoryDTOResponse categoryDTOResponse = _mapper.Map<CategoryDTOResponse>(category);

            Log.Information("Categories => {@categoryDTOResponse} => { Kategori Güncellendi. }", categoryDTOResponse);

            return Ok(Sonuc<CategoryDTOResponse>.SuccessWithData(categoryDTOResponse));
        }
        [HttpGet("/GetCategory/{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            Category category = await _categoryService.GetAsync(x => x.Id == categoryId);
            if (category == null)
            {
                return NotFound(Sonuc<CategoryDTOResponse>.SuccessNoDataFound());
            }

            CategoryDTOResponse categoryDTOResponse = _mapper.Map<CategoryDTOResponse>(category);

            Log.Information("Categories => {@categoryDTOResponse} => { Kategori Getirildi. }", categoryDTOResponse);

            return Ok(Sonuc<CategoryDTOResponse>.SuccessWithData(categoryDTOResponse));
        }
        [HttpGet("/GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync(x=>x.IsActive==true);
            if (categories == null)
            {
                return NotFound(Sonuc<CategoryDTOResponse>.SuccessNoDataFound());
            }
            List<CategoryDTOResponse> categoryDTOResponseList = new();
            foreach (var category in categories)
            {
                categoryDTOResponseList.Add(_mapper.Map<CategoryDTOResponse>(category));
            }

            Log.Information("Categories => {@categoryDTOResponse} => { Kategoriler Getirildi. }", categoryDTOResponseList);

            return Ok(Sonuc<List<CategoryDTOResponse>>.SuccessWithData(categoryDTOResponseList));
        }
    }
}
