using Core.Entities;
using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.Application.User
{
    public class RegisterDeliveryPersonRequestDTO
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("corporateNumber")]
        public string CorporateNumber { get; set; }
        [JsonPropertyName("dateOfBirth")]
        public DateOnly DateOfBirth { get; set; }
        [JsonPropertyName("driverLicense")]
        public string DriverLicense { get; set; }
        [JsonPropertyName("driverCardType")]
        public string DriverCardType { get; set; }


    }
}
