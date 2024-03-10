using ERPProject.Entity.Poco;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.ValidationRules.FluentValidation
{
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().MinimumLength(3).WithMessage("Ürün ismi 3 karakterden uzun olmalıdır.");
            RuleFor(x => x.Description).MinimumLength(2).NotEmpty().WithMessage("Ürün açıklaması girilmesi zorunludur.");
        }
    }
}
