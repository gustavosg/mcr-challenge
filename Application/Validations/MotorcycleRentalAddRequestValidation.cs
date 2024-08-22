using Application.DTO.Application.MotorcycleRental;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class MotorcycleRentalAddRequestValidation : AbstractValidator<MotorcycleRentalAddRequestDTO>
    {
        public MotorcycleRentalAddRequestValidation()
        {
            RuleFor(x => x.DateBegin)
                .Must(x => x.Day.Equals(DateTime.Now.AddDays(1).Day))
                .WithMessage("Date of Begin should be the next day of today")
                ;

            RuleFor(x => x.ExpectedDateEnd)
                .Must(p => !(p == DateOnly.FromDateTime(DateTime.MinValue)));

            RuleFor(x => x.RentalId)
                .Must(p => !(p.Equals(Guid.NewGuid())))
                .WithMessage("You need to select a Rental");


        }
    }
}
