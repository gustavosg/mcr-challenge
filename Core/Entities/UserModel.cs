using Core.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Core.Entities
{
    public class UserModel : IdentityUser<Guid>
    {
        [Description("Corporate Number - CNPJ")]
        public string? CorporateNumber { get; set; }
        public string? DriverLicense { get; set; }
        public DriverCardType DriverCardType { get; set; }
        public string? Photo { get; set; }
        public virtual List<UserRoleModel> UserRoles { get; set; }

        public bool IsDriverCardTypeA() => this.DriverCardType.Equals(DriverCardType.A);
    }
}
