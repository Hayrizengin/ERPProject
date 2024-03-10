using AutoMapper;
using ERPProject.Entity.DTO.InvoiceDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.InvoiceMap
{
    public class InvoiceDTORequestMap : Profile
    {
        public InvoiceDTORequestMap()
        {
            CreateMap<Invoice, InvoiceDTORequest>();
            CreateMap<InvoiceDTORequest, Invoice>();
        }
    }
}
