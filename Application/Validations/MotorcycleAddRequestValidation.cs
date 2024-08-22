using Application.DTO.Application.Motorcycle;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class MotorcycleAddRequestValidation : AbstractValidator<MotorcycleAddRequestDTO>
    {
        public MotorcycleAddRequestValidation()
        {
            RuleFor(x => x.Plate)
                .MinimumLength(7)
                .NotEmpty();

            RuleFor(x => x.Model)
                .NotEmpty();

        }
    }
}
