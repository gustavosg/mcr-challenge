using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.User
{
    public class AuthenticateResponseDTO : BaseEntity
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
