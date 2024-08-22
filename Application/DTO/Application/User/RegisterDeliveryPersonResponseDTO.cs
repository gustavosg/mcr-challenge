using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.Application.User
{
    public class RegisterDeliveryPersonResponseDTO
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
