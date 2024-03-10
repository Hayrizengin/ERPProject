using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.StockDTO;
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

    public class StockDetailController : Controller
    {
        private readonly IStockDetailService _stockDetailService;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        public StockDetailController(IMapper mapper, IStockDetailService stockDetailService, IStockService stockService, IUserService userService)
        {
            _mapper = mapper;
            _stockDetailService = stockDetailService;
            _stockService = stockService;
        }

        [HttpPost("/AddStockDetail")]
        public async Task<IActionResult> AddStockDetail(StockDetailDTORequest stockDetailDTORequest)
        {
            Stock stock = await _stockService.GetAsync(x => x.Id == stockDetailDTORequest.StockId);

            if ((stock.Quantity + stockDetailDTORequest.Quantity)>=0)
            {
                StockDetail stockDetail = _mapper.Map<StockDetail>(stockDetailDTORequest);
                await _stockDetailService.AddAsync(stockDetail);
                stock.Quantity += stockDetailDTORequest.Quantity;
                await _stockService.UpdateAsync(stock);
                StockDetailDTOResponse stockDetailDTOResponse = _mapper.Map<StockDetailDTOResponse>(stockDetail);
                Log.Information("StockDetails => {@stockDetailDTOResponse} => { Stok Detayı Eklendi }", stockDetailDTOResponse);
                return Ok(Sonuc<StockDetailDTOResponse>.SuccessWithData(stockDetailDTOResponse));
            }
            else
            {
                return BadRequest(Sonuc<StockDetailDTOResponse>.SuccessWithoutData());
            }
        }


        [HttpDelete("/RemoveStockDetail/{stockDetailId}")]
        public async Task<IActionResult> RemoveStockDetail(long stockDetailId)
        {
            StockDetail stockDetail = await _stockDetailService.GetAsync(x=>x.Id == stockDetailId);
            if (stockDetail == null)
            {
                return NotFound(Sonuc<StockDetailDTOResponse>.SuccessNoDataFound());
            }

            StockDetailDTOResponse stockDetailDTOResponse = _mapper.Map<StockDetailDTOResponse>(stockDetail);

            Log.Information("StockDetails => {@stockDetailDTOResponse} => { Stok Detayı Silindi. }", stockDetailDTOResponse);

            return Ok(Sonuc<StockDetailDTOResponse>.SuccessWithData(stockDetailDTOResponse));
        }

        [HttpPost("/UpdateStockDetail")]
        public async Task<IActionResult> UpdateStockDetail(StockDetailDTORequest stockDetailDTORequest)
        {
            StockDetail stockDetail = await _stockDetailService.GetAsync(x=>x.Id == stockDetailDTORequest.Id);
            if(stockDetail == null)
            {
                return NotFound(Sonuc<StockDetailDTOResponse>.SuccessNoDataFound());
            }

            stockDetail = _mapper.Map(stockDetailDTORequest,stockDetail);
            await _stockDetailService.UpdateAsync(stockDetail);

            StockDetailDTOResponse stockDetailDTOResponse = _mapper.Map<StockDetailDTOResponse>(stockDetail);

            Log.Information("StockDetails => {@stockDetailDTOResponse} => { Stok Detayı Güncellendi. }", stockDetailDTOResponse);

            return Ok(Sonuc<StockDetailDTOResponse>.SuccessWithData(stockDetailDTOResponse));
        }

        [HttpGet("/StockDetail/{stockDetailId}")]
        public async Task<IActionResult> GetStockDetail(long stockDetailId)
        {
            StockDetail stockDetail = await _stockDetailService.GetAsync(x=>x.Id == stockDetailId,"Stock.Product","User");
            if(stockDetail == null)
            {
                return NotFound(Sonuc<StockDetailDTOResponse>.SuccessNoDataFound());
            }

            StockDetailDTOResponse stockDetailDTOResponse = _mapper.Map<StockDetailDTOResponse>(stockDetail);

            Log.Information("StockDetails => {@stockDetailDTOResponse} => { Stok Detayı Getirildi. }", stockDetailDTOResponse);

            return Ok(Sonuc<StockDetailDTOResponse>.SuccessWithData(stockDetailDTOResponse));
        }

        [HttpGet("/StockDetails")]
        public async Task<IActionResult> GetStockDetails()
        {
            var stockDetails = await _stockDetailService.GetAllAsync(x => x.IsActive == true, "Stock.Product", "User");
            if (stockDetails == null)
            {
                return NotFound(Sonuc<StockDetailDTOResponse>.SuccessNoDataFound());
            }

            List<StockDetailDTOResponse> stockDetailDTOResponseList = new();
            foreach (var stockdetail in stockDetails)
            {
                stockDetailDTOResponseList.Add(_mapper.Map<StockDetailDTOResponse>(stockdetail));
            }

            Log.Information("StockDetails => {@stockDetailDTOResponse} => { Stok Detayları Getirildi. }", stockDetailDTOResponseList);

            return Ok(Sonuc<List<StockDetailDTOResponse>>.SuccessWithData(stockDetailDTOResponseList));
        }

        [HttpGet("/StockDetailsByStock/{stockId}")]
        public async Task<IActionResult> GetStockDetailsByStock(long stockId)
        {
            var stockDetails = await _stockDetailService.GetAllAsync(x => x.IsActive == true && x.StockId == stockId, "Stock.Product", "User");
            if (stockDetails == null)
            {
                return NotFound(Sonuc<StockDetailDTOResponse>.SuccessNoDataFound());
            }

            List<StockDetailDTOResponse> stockDetailDTOResponseList = new();
            foreach (var stockdetail in stockDetails)
            {
                stockDetailDTOResponseList.Add(_mapper.Map<StockDetailDTOResponse>(stockdetail));
            }

            Log.Information("StockDetails => {@stockDetailDTOResponse} => { Stoğa Göre Stok Detayları Getirildi. }", stockDetailDTOResponseList);

            return Ok(Sonuc<List<StockDetailDTOResponse>>.SuccessWithData(stockDetailDTOResponseList));
        }
        [AllowAnonymous]
        [HttpGet("/StockDetailsByUser/{userId}")]
        public async Task<IActionResult> GetStockDetailsByUser(long userId)
        {
            var stockDetails = await _stockDetailService.GetAllAsync(x => x.IsActive == true && x.RecieverId == userId, "Stock.Product", "User");
            if (stockDetails == null)
            {
                return NotFound(Sonuc<StockDetailDTOResponse>.SuccessNoDataFound());
            }

            List<StockDetailDTOResponse> stockDetailDTOResponseList = new();
            foreach (var stockdetail in stockDetails)
            {
                stockDetailDTOResponseList.Add(_mapper.Map<StockDetailDTOResponse>(stockdetail));
            }

            Log.Information("StockDetails => {@stockDetailDTOResponse} => { Stoğa Göre Stok Detayları Getirildi. }", stockDetailDTOResponseList);

            return Ok(Sonuc<List<StockDetailDTOResponse>>.SuccessWithData(stockDetailDTOResponseList));
        }
    }
}
