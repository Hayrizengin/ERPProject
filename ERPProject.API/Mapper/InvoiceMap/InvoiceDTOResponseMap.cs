using AutoMapper;
using ERPProject.Entity.DTO.InvoiceDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.InvoiceMap
{
    public class InvoiceDTOResponseMap : Profile
    {
        public InvoiceDTOResponseMap()
        {
            CreateMap<Invoice, InvoiceDTOResponse>().ReverseMap();
        }
    }
}
