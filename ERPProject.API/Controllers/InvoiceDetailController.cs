using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Entity.DTO.CompanyDTO;
using ERPProject.Entity.DTO.InvoiceDetailDTO;
using ERPProject.Entity.DTO.InvoiceDTO;
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
    public class InvoiceDetailController : Controller
    {
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IMapper _mapper;

        public InvoiceDetailController(IInvoiceDetailService invoiceDetailService, IMapper mapper)
        {
            _invoiceDetailService = invoiceDetailService;
            _mapper = mapper;
        }

        [HttpPost("/AddInvoiceDetail")]
        public async Task<IActionResult> AddInvoiceDetail(InvoiceDetailDTORequest invoiceDetailDTORequest)
        {
            InvoiceDetail invoiceDetail = _mapper.Map<InvoiceDetail>(invoiceDetailDTORequest);
            await _invoiceDetailService.AddAsync(invoiceDetail);

            InvoiceDetailDTOResponse invoiceDetailDTOResponse = _mapper.Map<InvoiceDetailDTOResponse>(invoiceDetail);

            Log.Information("InvoiceDetails => {@invoiceDetailDTOResponse} => { Fatura Detayı Eklendi. }", invoiceDetailDTOResponse);

            return Ok(Sonuc<InvoiceDetailDTOResponse>.SuccessWithData(invoiceDetailDTOResponse));
        }

        [HttpDelete("/RemoveInvoiceDetail/{invoiceDetailId}")]
        public async Task<IActionResult> RemoveInvoiceDetail(long invoiceDetailId)
        {
            InvoiceDetail invoiceDetail = await _invoiceDetailService.GetAsync(x=>x.Id == invoiceDetailId);
            if (invoiceDetail != null)
            {
                return NotFound(Sonuc<InvoiceDetailDTOResponse>.SuccessNoDataFound());
            }
            await _invoiceDetailService.RemoveAsync(invoiceDetail);

            Log.Information("InvoiceDetails => {@invoiceDetail} => { Fatura Detayı Silindi. }", invoiceDetail);

            return Ok(Sonuc<InvoiceDetailDTOResponse>.SuccessWithoutData());
        }

        [HttpPost("/UpdateInvoiceDetail")]
        public async Task<IActionResult> UpdateInvoiceDetail(InvoiceDetailDTORequest invoiceDetailDTORequest)
        {
            InvoiceDetail invoiceDetail = await _invoiceDetailService.GetAsync(x=>x.Id == invoiceDetailDTORequest.Id);
            if (invoiceDetail != null)
            {
                return NotFound(Sonuc<InvoiceDetailDTOResponse>.SuccessNoDataFound());
            }

            invoiceDetail = _mapper.Map(invoiceDetailDTORequest,invoiceDetail);
            await _invoiceDetailService.UpdateAsync(invoiceDetail);

            InvoiceDetailDTOResponse invoiceDetailDTOResponse = _mapper.Map<InvoiceDetailDTOResponse>(invoiceDetail);
            Log.Information("InvoiceDetails => {@invoiceDetailDTOResponse} => { Fatura Detayı Güncellendi. }", invoiceDetailDTOResponse);

            return Ok(Sonuc<InvoiceDetailDTOResponse>.SuccessWithData(invoiceDetailDTOResponse));
        }

        [HttpGet("/GetInvoiceDetail/{invoiceDetailId}")]
        public async Task<IActionResult> GetInvoiceDetail(long invoiceDetailId)
        {
            InvoiceDetail invoiceDetail = await _invoiceDetailService.GetAsync(x=>x.Id == invoiceDetailId);
            if (invoiceDetail != null)
            {
                return NotFound(Sonuc<InvoiceDetailDTOResponse>.SuccessNoDataFound());
            }
            InvoiceDetailDTOResponse invoiceDetailDTOResponse = _mapper.Map<InvoiceDetailDTOResponse>(invoiceDetail);

            Log.Information("InvoiceDetails => {@invoiceDetailDTOResponse} => { Fatura Detayı Getirildi. }", invoiceDetailDTOResponse);

            return Ok(Sonuc<InvoiceDetailDTOResponse>.SuccessWithData(invoiceDetailDTOResponse));
        }

        [HttpGet("/GetInvoiceDetails")]
        public async Task<IActionResult> GetInvoiceDetails()
        {
            var invoiceDetails = await _invoiceDetailService.GetAllAsync(x=>x.IsActive == true,"Invoice");
            if (invoiceDetails == null)
            {
                return NotFound(Sonuc<InvoiceDetailDTOResponse>.SuccessNoDataFound());
            }
            List<InvoiceDetailDTOResponse> invoiceDetailDTOResponseList = new();
            foreach (var item in invoiceDetails)
            {
                invoiceDetailDTOResponseList.Add(_mapper.Map<InvoiceDetailDTOResponse>(item));
            }

            Log.Information("InvoiceDetails => {@invoiceDetailDTOResponse} => { Fatura Detayları Getirildi. } ", invoiceDetailDTOResponseList);

            return Ok(Sonuc<List<InvoiceDetailDTOResponse>>.SuccessWithData(invoiceDetailDTOResponseList));
        }

        [HttpGet("/GetInvoiceDetailsByInvoice/{invoiceId}")]
        public async Task<IActionResult> GetInvoiceDetailsByInvoice(long invoiceId)
        {
            var invoiceDetails = await _invoiceDetailService.GetAllAsync(x=>x.InvoiceId == invoiceId);
            if (invoiceDetails != null)
            {
                return NotFound(Sonuc<InvoiceDetailDTOResponse>.SuccessNoDataFound());
            }
            List<InvoiceDetailDTOResponse> invoiceDetailDTOResponseList = new();
            foreach (var item in invoiceDetails)
            {
                invoiceDetailDTOResponseList.Add(_mapper.Map<InvoiceDetailDTOResponse>(item));
            }

            Log.Information("InvoiceDetails => {@invoiceDetailDTOResponse} => { Fatura Detayları Faturaya Göre Getirildi. } ", invoiceDetailDTOResponseList);

            return Ok(Sonuc<List<InvoiceDetailDTOResponse>>.SuccessWithData(invoiceDetailDTOResponseList));
        }
    }
}
