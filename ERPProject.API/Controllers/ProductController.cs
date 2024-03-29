﻿using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.ProductDTO;
using ERPProject.Entity.DTO.UserDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ERPProject.API.Controllers
{
    [Route("[action]")]
    [ApiController]
    [Authorize(Roles = "Admin,Satın Alma İşlemleri,Ürün Görüntüleme")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpPost("/AddProduct")]
        [ValidationFilter(typeof(ProductValidator))]
        public async Task<IActionResult> AddProduct(ProductDTORequest productDTORequest)
        {
            Product product = _mapper.Map<Product>(productDTORequest);

            await _productService.AddAsync(product);

            ProductDTOResponse productDTOResponse = _mapper.Map<ProductDTOResponse>(product);

            Log.Information("Products => {@productDTOResponse} => { Ürün Eklendi. }", productDTOResponse);

            return Ok(Sonuc<ProductDTOResponse>.SuccessWithData(productDTOResponse));
        }
        [HttpDelete("/RemoveProduct/{productId}")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            Product product = await _productService.GetAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound(Sonuc<ProductDTOResponse>.SuccessNoDataFound());
            }

            await _productService.RemoveAsync(product);

            Log.Information("Products => {@product} => { Ürün Silindi. } ", product);

            return Ok(Sonuc<ProductDTOResponse>.SuccessWithoutData());
        }

        [HttpPost("/UpdateProduct")]
        [ValidationFilter(typeof(ProductValidator))]
        public async Task<IActionResult> UpdateProduct(ProductDTORequest productDTORequest)
        {
            Product product = await _productService.GetAsync(x => x.Id == productDTORequest.Id);
            if (product == null)
            {
                return NotFound(Sonuc<ProductDTOResponse>.SuccessNoDataFound());
            }
            product = _mapper.Map(productDTORequest, product);

            await _productService.UpdateAsync(product);

            ProductDTOResponse productDTOResponse = _mapper.Map<ProductDTOResponse>(product);

            Log.Information("Products => {@productDTOResponse} => { Ürün Güncellendi. }", productDTOResponse);

            return Ok(Sonuc<ProductDTOResponse>.SuccessWithData(productDTOResponse));
        }

        [HttpGet("/GetProduct/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct(int productId)
        {
            Product product = await _productService.GetAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound(Sonuc<ProductDTOResponse>.SuccessNoDataFound());
            }

            ProductDTOResponse productDTOResponse = _mapper.Map<ProductDTOResponse>(product);

            Log.Information("Products => {@productDTOResponse} => { Ürün Getirildi. }", productDTOResponse);

            return Ok(Sonuc<ProductDTOResponse>.SuccessWithData(productDTOResponse));
        }
        [AllowAnonymous]
        [HttpGet("/GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllAsync(x => x.IsActive == true, "Category", "Brand");
            if (products == null)
            {
                return NotFound(Sonuc<ProductDTOResponse>.SuccessNoDataFound());
            }

            List<ProductDTOResponse> productDTOResponseList = new();
            foreach (var product in products)
            {
                productDTOResponseList.Add(_mapper.Map<ProductDTOResponse>(product));
            }

            Log.Information("Products => {@productDTOResponse} => { Ürünler Getirildi. }", productDTOResponseList);

            return Ok(Sonuc<List<ProductDTOResponse>>.SuccessWithData(productDTOResponseList));
        }

        [HttpGet("/GetProductsByCategory/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetAllAsync(x => x.IsActive == true && x.CategoryId == categoryId, "Category", "Brand");
            if (products == null)
            {
                return NotFound(Sonuc<ProductDTOResponse>.SuccessNoDataFound());
            }

            List<ProductDTOResponse> productDTOResponseList = new();
            foreach (var product in products)
            {
                productDTOResponseList.Add(_mapper.Map<ProductDTOResponse>(product));
            }
            Log.Information("Products => {@productDTOResponse} => { Kategoriye Göre Ürünler Getirildi. }", productDTOResponseList);
            return Ok(Sonuc<List<ProductDTOResponse>>.SuccessWithData(productDTOResponseList));
        }
        [HttpGet("/GetProductsByBrand/{brandId}")]
        public async Task<IActionResult> GetProductsByBrand(int brandId)
        {
            var products = await _productService.GetAllAsync(x => x.IsActive == true && x.BrandId == brandId, "Category", "Brand");
            if (products == null)
            {
                return NotFound(Sonuc<ProductDTOResponse>.SuccessNoDataFound());
            }
            List<ProductDTOResponse> productDTOResponseList = new();
            foreach (var product in products)
            {
                productDTOResponseList.Add(_mapper.Map<ProductDTOResponse>(product));
            }
            Log.Information("Products => {@productDTOResponse} => { Markaya Göre Ürünler Getirildi. }", productDTOResponseList);
            return Ok(Sonuc<List<ProductDTOResponse>>.SuccessWithData(productDTOResponseList));
        }
    }
}
