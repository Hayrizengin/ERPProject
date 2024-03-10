using ERPProject.Entity.Poco;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.ValidationRules.FluentValidation
{
    public class StockValidator:AbstractValidator<Stock>
    {
        public StockValidator()
        {
            RuleFor(x=>x.Quantity).NotEmpty().WithMessage("Stok miktar bilgisi girmek zorunludur.");
            RuleFor(x=>x.Description).NotEmpty().WithMessage("Stok açıklaması zorunludur.");
        }
    }
}
