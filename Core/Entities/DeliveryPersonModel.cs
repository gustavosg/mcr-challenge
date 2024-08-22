using Core.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("DeliveryPersons")]
    public class DeliveryPersonModel : BaseEntity
    {
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string DriverLicense { get; set; }
        public DriverCardType DriverCardType { get; set; }
        public string DriverPhoto { get; set; }
    }
}
