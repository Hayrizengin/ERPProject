using ERPProject.Entity.Poco;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.ValidationRules.FluentValidation
{
    public class CompanyValidator:AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty().WithMessage("Şirket ismi boş bırakılamaz.");
        }
    }
}
