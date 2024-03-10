using AutoMapper;
using ERPProject.Entity.DTO.InvoiceDetailDTO;
using ERPProject.Entity.Poco;

namespace ERPProject.API.Mapper.InvoiceDetailMap
{
    public class InvoiceDetailDTOResponseMap:Profile
    {
        public InvoiceDetailDTOResponseMap()
        {
            CreateMap<InvoiceDetail, InvoiceDetailDTOResponse>();
            CreateMap<InvoiceDetailDTOResponse, InvoiceDetail>();
        }
    }
}
