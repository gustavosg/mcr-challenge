using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.User
{
    public class UserDto : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
