using ERPProject.Entity.Poco;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Business.ValidationRules.FluentValidation
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x=>x.Name).MinimumLength(2).NotEmpty().WithMessage("İsim boş olamaz.");
            RuleFor(x=>x.LastName).MinimumLength(1).NotEmpty().WithMessage("Soyad boş olamaz.");
            RuleFor(x=>x.Phone).Length(11).NotEmpty().WithMessage("Telefon numarası boş ve 11 karakterden uzun olamaz.");
            RuleFor(x=>x.Email).MinimumLength(12).NotEmpty().WithMessage("Email boş olamaz.");
            RuleFor(x=>x.Password).NotEmpty().WithMessage("Şifre boş olamaz.");
        }
    }
}
