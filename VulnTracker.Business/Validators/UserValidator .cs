using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulnTracker.Domain.Entities;

namespace VulnTracker.Business.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalı");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email adresi girin");

            RuleFor(u => u.PasswordHash)
                .NotEmpty().WithMessage("Şifre boş olamaz")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı");
        }
    }
}
