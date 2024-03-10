using ERPProject.Entity.Poco;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.ValidationRules.FluentValidation
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x=>x.Name).MaximumLength(40).NotEmpty().WithMessage("Kategori ismi boş olamaz.");
        }
    }
}
