using ERPProject.Entity.Poco;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.ValidationRules.FluentValidation
{
    public class OfferValidator:AbstractValidator<Offer>
    {
        public OfferValidator()
        {
            RuleFor(x=>x.SupplierName).MaximumLength(60).NotEmpty().WithMessage("Tedarikçi ismi boş olamaz.");
            RuleFor(x => x.Description).MinimumLength(2).NotEmpty().WithMessage("Açıklama 2 karakterden daha uzun olmalıdır.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat bilgisi boş olamaz.");
        }
    }
}
