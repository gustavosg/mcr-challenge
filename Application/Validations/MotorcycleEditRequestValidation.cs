using Application.DTO.Application.Motorcycle;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class MotorcycleEditRequestValidation : AbstractValidator<MotorcycleEditRequestDTO>
    {
        public MotorcycleEditRequestValidation()
        {
            RuleFor(x => x.Plate)
                .MinimumLength(8)
                .NotEmpty();
        }
    }
}
