using Application.DTO.Application.User;
using Core.Enum;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class RegisterDeliveryPersonRequestValidation : AbstractValidator<RegisterDeliveryPersonRequestDTO>
    {
        public RegisterDeliveryPersonRequestValidation() {
            RuleFor(x => x.UserName).NotEmpty();

            RuleFor(x => x.CorporateNumber)
                .NotEmpty()
                .MinimumLength(14);

            RuleFor(x => x.DriverLicense)
                .NotEmpty();

            RuleFor(x => x.DriverCardType)
                .Must(i => new string[] { "A", "B", "AB" }.Contains(i))
                .WithMessage("Possible values: A, B, AB")
                ;

            RuleFor(x => x.DateOfBirth)
                .Must(p => !(p == DateOnly.FromDateTime(DateTime.MinValue)))
                .NotEmpty();
        }
    }
}
