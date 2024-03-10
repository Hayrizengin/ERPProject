using ERPProject.Entity.Poco;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.ValidationRules.FluentValidation
{
    public class RequestValidator:AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Istek başlığı boş olamaz.");
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3).WithMessage("Istek açıklaması 3 karakterden uzun olmalıdır.");
        }
    }
}
