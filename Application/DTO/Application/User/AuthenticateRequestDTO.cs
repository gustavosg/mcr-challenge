using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.User
{
    public class AuthenticateRequestDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
