using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.RoleDTO;
using ERPProject.Entity.DTO.StockDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace ERPProject.API.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Authorize(Roles = "Admin,Satın Alma İşlemleri,Ürün Görüntüleme")]

    public class StockController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;

        public StockController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        [HttpGet("/Stocks")]
        public async Task<IActionResult> GetStocks()
        {
            var stocks = await _stockService.GetAllAsync(x=>x.IsActive == true ,"Product","Company");
            if (stocks == null)
            {
                return NotFound(Sonuc<StockDTOResponse>.SuccessNoDataFound());
            }

            List<StockDTOResponse> stockDTOResponses = new();
            foreach (var stock in stocks)
            {
                stockDTOResponses.Add(_mapper.Map<StockDTOResponse>(stock));
            }

            Log.Information("Stocks => {@stockDTOResponse} => { Stoklar Getirildi. }", stockDTOResponses);
            return Ok(Sonuc<List<StockDTOResponse>>.SuccessWithData(stockDTOResponses));
        }

        [HttpGet("/Stock/{stockId}")]
        public async Task<IActionResult> GetStock(int stockId)
        {
            var stock = await _stockService.GetAsync(e=>e.Id== stockId && e.IsActive == true, "Product", "Company");
            if (stock == null)
            {
                return NotFound(Sonuc<StockDTOResponse>.SuccessNoDataFound());
            }
            StockDTOResponse stockDTOResponse = _mapper.Map<StockDTOResponse>(stock);

            Log.Information("Stocks => {@stockDTOResponse} => { Stok Getirildi. }", stockDTOResponse);
            return Ok(Sonuc<StockDTOResponse>.SuccessWithData(stockDTOResponse));

        }

        [HttpPost("/AddStock")]
        [ValidationFilter(typeof(StockValidator))]
        public async Task<IActionResult> AddStock(StockDTORequest stockDTORequest)
        {
            var innerstock = await _stockService.GetAsync(x=>x.CompanyId==stockDTORequest.CompanyId &&x.ProductId==stockDTORequest.ProductId);

            if (innerstock != null) 
            {
                innerstock.Quantity += stockDTORequest.Quantity;
                await _stockService.UpdateAsync(innerstock);
                StockDTOResponse innerStockDTOResponse = _mapper.Map<StockDTOResponse>(innerstock);
                Log.Information("Stocks => {@stockDTOResponse} => { Stok Eklendi. }", innerStockDTOResponse);
                return Ok(Sonuc<StockDTOResponse>.SuccessWithData(innerStockDTOResponse));
            }
            var stock = _mapper.Map<Stock>(stockDTORequest);
            await _stockService.AddAsync(stock);
            StockDTOResponse stockDTOResponse = _mapper.Map<StockDTOResponse>(stock);

            Log.Information("Stocks => {@stockDTOResponse} => { Stok Eklendi. }", stockDTOResponse);

            return Ok(Sonuc<StockDTOResponse>.SuccessWithData(stockDTOResponse));
        }

        [HttpPost("/UpdateStock")]
        [ValidationFilter(typeof(StockValidator))]
        public async Task<IActionResult> UpdateStock(StockDTORequest stockDTORequest)
        {
            var stock = await _stockService.GetAsync(e=>e.Id== stockDTORequest.Id);
            if (stock == null)
            {
                return NotFound(Sonuc<StockDTOResponse>.SuccessNoDataFound());
            }

            _mapper.Map(stockDTORequest, stock);
            await _stockService.UpdateAsync(stock);

            StockDTOResponse stockDTOResponse = _mapper.Map<StockDTOResponse>(stock);

            Log.Information("Stocks => {@stockDTOResponse} => { Stok Güncellendi. }", stockDTOResponse);

            return Ok(Sonuc<StockDTOResponse>.SuccessWithData(stockDTOResponse));
            
        }

        [HttpDelete("/RemoveStock/{stockId}")]
        public async Task<IActionResult> RemoveStock(int stockId)
        {
            var stock = await _stockService.GetAsync(e=>e.Id== stockId);
            if (stock == null)
            {
                return NotFound(Sonuc<StockDTOResponse>.SuccessNoDataFound());
            }
            await _stockService.RemoveAsync(stock);

            Log.Information("Stocks => {@stock} => { Stok Silindi. }", stock);

            return Ok(Sonuc<StockDTOResponse>.SuccessWithoutData());

        }

        [HttpGet("/StocksByCompany/{companyId}")]
        public async Task<IActionResult> GetStocksByCompany(int companyId)
        {

            var stocks = await _stockService.GetAllAsync(x=>x.CompanyId == companyId,"Company","Product");
            if (stocks == null)
            {
                return NotFound(Sonuc<StockDTOResponse>.SuccessNoDataFound());
            }

            List<StockDTOResponse> stockDTOResponses = new();
            foreach (var stock in stocks)
            {
                stockDTOResponses.Add(_mapper.Map<StockDTOResponse>(stock));
            }

            Log.Information("Stocks => {@stockDTOResponse} => { Stoklar Getirildi. }", stockDTOResponses);
            return Ok(Sonuc<List<StockDTOResponse>>.SuccessWithData(stockDTOResponses));
        }

    }
}
