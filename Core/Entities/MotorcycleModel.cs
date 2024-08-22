using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Motorcycles")]
    public class MotorcycleModel : BaseEntity
    {
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
    }
}
