using AutoMapper;
using ERPProject.Business.Abstract;
using ERPProject.Business.ValidationRules.FluentValidation;
using ERPProject.Core.Aspects;
using ERPProject.Entity.DTO.InvoiceDTO;
using ERPProject.Entity.DTO.OfferDTO;
using ERPProject.Entity.Poco;
using ERPProject.Entity.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net.Mail;

namespace ERPProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Satın Alma İşlemleri")]

    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        private readonly IRequestService _requestService;

        public OfferController(IMapper mapper, IOfferService offerService, IRequestService requestService)
        {
            _mapper = mapper;
            _offerService = offerService;
            _requestService = requestService;
        }

        [HttpPost("/AddOffer")]
        [ValidationFilter(typeof(OfferValidator))]
        public async Task<IActionResult> AddOffer(OfferDTORequest offerDTORequest)
        {
            Offer offer = _mapper.Map<Offer>(offerDTORequest);
            await _offerService.AddAsync(offer);

            OfferDTOResponse offerDTOResponse = _mapper.Map<OfferDTOResponse>(offer);

            Log.Information("Offers => {@offerDTOResponse} => { Teklif Eklendi. }", offerDTOResponse);

            return Ok(Sonuc<OfferDTOResponse>.SuccessWithData(offerDTOResponse));
        }
        [HttpDelete("/RemoveOffer/{offerId}")]
        public async Task<IActionResult> RemoveOffer(int offerId)
        {
            Offer offer = await _offerService.GetAsync(x => x.Id == offerId);
            if (offer == null)
            {
                return NotFound(Sonuc<OfferDTOResponse>.SuccessNoDataFound());
            }

            await _offerService.RemoveAsync(offer);

            Log.Information("Offers => {@offer} => { Teklif Silindi. }", offer);

            return Ok(Sonuc<OfferDTOResponse>.SuccessWithoutData());
        }

        [HttpPost("/UpdateOffer")]
        //[ValidationFilter(typeof(OfferValidator))]
        public async Task<IActionResult> UpdateOffer(OfferDTORequest offerDTORequest)
        {
            Offer offer = await _offerService.GetAsync(x => x.Id == offerDTORequest.Id);
            if (offer == null)
            {
                return NotFound(Sonuc<OfferDTOResponse>.SuccessNoDataFound());
            }
            offer = _mapper.Map(offerDTORequest, offer);
            await _offerService.UpdateAsync(offer);

            OfferDTOResponse offerDTOResponse = _mapper.Map<OfferDTOResponse>(offer);

            Log.Information("Offers => {@offerDTOResponse} => { Teklif Güncellendi. }", offerDTOResponse);

            return Ok(Sonuc<OfferDTOResponse>.SuccessWithData(offerDTOResponse));
        }
        [HttpGet("/GetOffer/{offerId}")]
        public async Task<IActionResult> GetOffer(int offerId)
        {
            Offer offer = await _offerService.GetAsync(x => x.Id == offerId, "User", "Request");
            if (offer == null)
            {
                return NotFound(Sonuc<OfferDTOResponse>.SuccessNoDataFound());
            }

            OfferDTOResponse offerDTOResponse = _mapper.Map<OfferDTOResponse>(offer);

            Log.Information("Offers => {@offerDTOResponse} => { Teklif Getirildi. }", offerDTOResponse);
            //Log.Information($"Offers => {offerDTOResponse} =>  Teklif Getirildi.");

            return Ok(Sonuc<OfferDTOResponse>.SuccessWithData(offerDTOResponse));
        }

        [HttpGet("/GetOffers")]
        public async Task<IActionResult> GetOffers()
        {

            var offers = await _offerService.GetAllAsync(x => x.IsActive == true, "User", "Request");
            if (offers == null)
            {
                return NotFound(Sonuc<OfferDTOResponse>.SuccessNoDataFound());
            }
            List<OfferDTOResponse> offerDTOResponseList = new();
            foreach (var offer in offers)
            {
                offerDTOResponseList.Add(_mapper.Map<OfferDTOResponse>(offer));
            }

            Log.Information("Offers => {@offerDTOResponse} => { Teklifleri Getir. }", offerDTOResponseList);
            return Ok(Sonuc<List<OfferDTOResponse>>.SuccessWithData(offerDTOResponseList));
        }
        [HttpPost("/UpdateAllOffer")]
        public async Task<IActionResult> UpdateAll(OfferDTORequest offerDTORequest)
        {
            var request = await _requestService.GetAsync(e => e.Id == offerDTORequest.RequestId, "User", "AcceptedUser","Product");
            request.RequestStatus = 2;
            await _requestService.UpdateAsync(request);
            var offer = _mapper.Map<Offer>(offerDTORequest);
            var response = await _offerService.UpdateAllAsync(offer);
            OfferDTOResponse offerDTOResponse1 = _mapper.Map<OfferDTOResponse>(offer);




            string AcceptRequestMessage = request.User.Name + " " + request.User.LastName + " adlı personelimiz " + request.Title + " başlıklı isteğiniz tamamlanmıştır.";
            string RefuseRequestMessage = request.User.Name + " " + request.User.LastName + " adlı personelimiz " + request.Title + " başlıklı isteğiniz reddedildi";
            if (request.RequestStatus == 2)
            {
                SendMail(request.User.Email, AcceptRequestMessage);

            }
            if (request.RequestStatus == 3)
            {
                SendMail(request.User.Email, RefuseRequestMessage);
            }


            Log.Information("Offers => {@offerDTOResponse} => { Teklifler Toplu Güncellendi }", offerDTOResponse1);
            return Ok(Sonuc<OfferDTOResponse>.SuccessWithData(offerDTOResponse1));


        }

        [HttpGet("/GetOffersByRequest/{requestId}")]
        public async Task<IActionResult> GetOffersByRequest(long requestId)
        {
            var offers = await _offerService.GetAllAsync(x => x.IsActive == true && x.RequestId == requestId, "User", "Request");
            if (offers == null)
            {
                return NotFound(Sonuc<OfferDTOResponse>.SuccessNoDataFound());
            }
            List<OfferDTOResponse> offerDTOResponseList = new();
            foreach (var offer in offers)
            {
                offerDTOResponseList.Add(_mapper.Map<OfferDTOResponse>(offer));
            }

            Log.Information("Offers => {@offerDTOResponse} => { Teklifleri İsteklere Göre Getir. }", offerDTOResponseList);
            return Ok(Sonuc<List<OfferDTOResponse>>.SuccessWithData(offerDTOResponseList));
        }
        [HttpGet("/GetOfferByjs/{requestId}")]
        public async Task<IActionResult> GetOfferByjs(long requestId)
        {
            var offerlist = await _offerService.GetAllAsync(x => x.RequestId == requestId && x.IsActive == true, "User", "Request");
            if (offerlist == null)
            {
                return NotFound(Sonuc<OfferDTOResponse>.SuccessNoDataFound());
            }

            List<OfferDTOResponse> offerDTOResponseList = new();

            foreach (var offer in offerlist)
            {
                offerDTOResponseList.Add(_mapper.Map<OfferDTOResponse>(offer));
            }

            Log.Information("Offers => {@offerDTOResponse} => { Teklif Getirildi. }", offerDTOResponseList);
            //Log.Information($"Offers => {offerDTOResponse} =>  Teklif Getirildi.");

            return Ok(offerDTOResponseList);
        }
        private void SendMail(string mail, string body)
        {

            MailMessage mesaj = new MailMessage();
            mesaj.From = new MailAddress("teklifbilgilendirme@hotmail.com");
            mesaj.To.Add(mail);
            mesaj.Subject = "İstek Sonuçlandı";
            mesaj.Body = body;

            SmtpClient a = new SmtpClient();
            a.Credentials = new System.Net.NetworkCredential("teklifbilgilendirme@hotmail.com", "Hakanceylan123");
            a.Port = 587;
            a.Host = "smtp.office365.com";
            a.EnableSsl = true;
            object userState = mesaj;


            a.Send(mesaj);
        }
    }
}
