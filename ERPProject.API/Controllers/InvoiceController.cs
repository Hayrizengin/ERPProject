using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Entity.DTO.DepartmentDTO;
using ERPProject.Entity.DTO.InvoiceDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ERPProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public InvoiceController(IMapper mapper, IInvoiceService invoiceService, IUserService userService, IDepartmentService departmentService, ICompanyService companyService)
        {
            _mapper = mapper;
            _invoiceService = invoiceService;
            _userService = userService;
            _departmentService = departmentService;
            _companyService = companyService;
        }
        [HttpPost("/AddInvoice")]
        [Authorize(Roles = "Admin,Muhasebe İşlemleri")]
        public async Task<IActionResult> AddInvoice(InvoiceDTORequest invoiceDTORequest)
        {
            Invoice invoice = _mapper.Map<Invoice>(invoiceDTORequest);
            await _invoiceService.AddAsync(invoice);

            InvoiceDTOResponse invoiceDTOResponse = _mapper.Map<InvoiceDTOResponse>(invoice);

            Log.Information("Invoices => {@invoiceDTOResponse} => { Fatura Eklendi. }", invoiceDTOResponse);

            return Ok(Sonuc<InvoiceDTOResponse>.SuccessWithData(invoiceDTOResponse));
        }
        [HttpDelete("/RemoveInvoice/{invoiceId}")]
        [Authorize(Roles = "Admin,Muhasebe İşlemleri")]
        public async Task<IActionResult> RemoveInvoice(int invoiceId)
        {
            Invoice invoice = await _invoiceService.GetAsync(x => x.Id == invoiceId);
            if (invoice == null)
            {
                return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
            }

            await _invoiceService.RemoveAsync(invoice);

            Log.Information("Invoices => {@invoice} => { Fatura Silindi. }", invoice);

            return Ok(Sonuc<InvoiceDTOResponse>.SuccessWithoutData());
        }

        [HttpPost("/UpdateInvoice")]
        [Authorize(Roles = "Admin,Muhasebe İşlemleri")]
        public async Task<IActionResult> UpdateInvoice(InvoiceDTORequest invoiceDTORequest)
        {
            Invoice invoice = await _invoiceService.GetAsync(x => x.Id == invoiceDTORequest.Id);
            if (invoice == null)
            {
                return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
            }
            invoice = _mapper.Map(invoiceDTORequest, invoice);
            await _invoiceService.UpdateAsync(invoice);

            //InvoiceDTOResponse invoiceDTOResponse = _mapper.Map<InvoiceDTOResponse>(invoice);

            Log.Information("Invoices => {@invoiceDTOResponse} => { Fatura Güncellendi. }", invoice);

            return Ok(Sonuc<Invoice>.SuccessWithData(invoice));
        }
        
        [HttpGet("/GetInvoice/{invoiceId}")]
        [Authorize(Roles = "Admin,Muhasebe İşlemleri,Muhasebe Görüntüleme")]
        public async Task<IActionResult> GetInvoice(int invoiceId)
        {
            Invoice invoice = await _invoiceService.GetAsync(x => x.Id == invoiceId);
            if (invoice == null)
            {
                return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
            }

            InvoiceDTOResponse invoiceDTOResponse = _mapper.Map<InvoiceDTOResponse>(invoice);

            Log.Information("Invoices => {@invoiceDTOResponse} => { Fatura Getirildi. }", invoiceDTOResponse);

            return Ok(Sonuc<InvoiceDTOResponse>.SuccessWithData(invoiceDTOResponse));
        }

        [HttpGet("/GetInvoices")]
        [Authorize(Roles = "Admin,Muhasebe İşlemleri,Muhasebe Görüntüleme")]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _invoiceService.GetAllAsync(x => x.IsActive == true);
            if (invoices == null)
            {
                return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
            }

            List<InvoiceDTOResponse> invoiceDTOResponseList = new();
            foreach (var invoice in invoices)
            {
                invoiceDTOResponseList.Add(_mapper.Map<InvoiceDTOResponse>(invoice));
            }

            Log.Information("Invoices => {@invoiceDTOResponse} => { Faturalar Getirildi. }", invoiceDTOResponseList);

            return Ok(Sonuc<List<InvoiceDTOResponse>>.SuccessWithData(invoiceDTOResponseList));
        }
        [HttpGet("/GetInvoicesByDate/{date}")]
        [Authorize(Roles = "Admin,Muhasebe İşlemleri,Muhasebe Görüntüleme")]
        public async Task<IActionResult> GetInvoicesByDate(string date)
        {
            string[] parcalar = date.Split('-');
            DateTime startDate = Convert.ToDateTime(parcalar[0]);
            DateTime endDate = Convert.ToDateTime(parcalar[1]);
            var invoices = await _invoiceService.GetAllAsync(x => x.IsActive == true && x.InvoiceDate >= startDate && x.InvoiceDate <= endDate);
            if (invoices == null)
            {
                return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
            }

            List<InvoiceDTOResponse> invoiceDTOResponseList = new();
            foreach (var invoice in invoices)
            {
                invoiceDTOResponseList.Add(_mapper.Map<InvoiceDTOResponse>(invoice));
            }

            Log.Information("Invoices => {@invoiceDTOResponse} => { Faturalar Getirildi. }", invoiceDTOResponseList);

            return Ok(Sonuc<List<InvoiceDTOResponse>>.SuccessWithData(invoiceDTOResponseList));
        }
        //[HttpGet("/GetInvoicesByOffer/{offerId}")]
        //public async Task<IActionResult> GetInvoicesByOffer(int offerId)
        //{
        //    var invoices = await _invoiceService.GetAllAsync(x => x.IsActive == true && x.OfferId == offerId, "Offer", "Company","Product");
        //    if (invoices == null)
        //    {
        //        return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
        //    }

        //    List<InvoiceDTOResponse> invoiceDTOResponseList = new();
        //    foreach (var invoice in invoices)
        //    {
        //        invoiceDTOResponseList.Add(_mapper.Map<InvoiceDTOResponse>(invoice));
        //    }

        //    Log.Information("Invoices => {@invoiceDTOResponse} => { Teklife Göre Faturalar Getirildi. }", invoiceDTOResponseList);

        //    return Ok(Sonuc<List<InvoiceDTOResponse>>.SuccessWithData(invoiceDTOResponseList));
        //}
        [HttpGet("/GetInvoicesByCompany/{userId}")]
        [Authorize(Roles = "Admin,Muhasebe İşlemleri,Muhasebe Görüntüleme")]
        public async Task<IActionResult> GetInvoicesByCompany(long userId)
        {
            User user = await _userService.GetAsync(x => x.Id == userId);
            Department department = await _departmentService.GetAsync(x => x.Id == user.DepartmentId);
            Company company = await _companyService.GetAsync(x => x.Id == department.CompanyId);

            var invoices = await _invoiceService.GetAllAsync(x => x.IsActive == true && x.CompanyName == company.Name);
            if (invoices == null)
            {
                return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
            }

            List<InvoiceDTOResponse> invoiceDTOResponseList = new();
            foreach (var invoice in invoices)
            {
                invoiceDTOResponseList.Add(_mapper.Map<InvoiceDTOResponse>(invoice));
            }

            Log.Information("Invoices => {@invoiceDTOResponse} => { Şirkete Göre Faturalar Getirildi. }", invoiceDTOResponseList);

            return Ok(Sonuc<List<InvoiceDTOResponse>>.SuccessWithData(invoiceDTOResponseList));
        }
        //[HttpGet("/GetInvoicesByProduct/{productId}")]
        //public async Task<IActionResult> GetInvoicesByProduct(int productId)
        //{
        //    var invoices = await _invoiceService.GetAllAsync(x => x.IsActive == true && x.ProductId == productId, "Offer", "Company", "Product");
        //    if (invoices == null)
        //    {
        //        return NotFound(Sonuc<InvoiceDTOResponse>.SuccessNoDataFound());
        //    }

        //    List<InvoiceDTOResponse> invoiceDTOResponseList = new();
        //    foreach (var invoice in invoices)
        //    {
        //        invoiceDTOResponseList.Add(_mapper.Map<InvoiceDTOResponse>(invoice));
        //    }

        //    Log.Information("Invoices => {@invoiceDTOResponse} => { Ürüne Göre Faturalar Getirildi. }", invoiceDTOResponseList);

        //    return Ok(Sonuc<List<InvoiceDTOResponse>>.SuccessWithData(invoiceDTOResponseList));
        //}


    }
}
