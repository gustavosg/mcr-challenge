using Application.DTO.Application.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class RegisterAdminRequestValidation : AbstractValidator<RegisterAdminRequestDTO>
    {
        public RegisterAdminRequestValidation() {
            RuleFor(x => x.UserName).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
