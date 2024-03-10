using ERPProject.Entity.DTO.StockDetailDTO;
using ERPProject.Entity.DTO.StockDTO;

namespace ERPProject.UI.Areas.Admin.Models
{
    public class StockDetailVM
    {
        public virtual ICollection<StockDTOResponse> Stocks { get; set; } = new List<StockDTOResponse>();
        public virtual ICollection<StockDetailDTOResponse> StockDetails { get; set; } = new List<StockDetailDTOResponse>();
    }
}
