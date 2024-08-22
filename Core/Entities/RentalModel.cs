using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Rentals")]
    public class RentalModel : BaseEntity
    {
        public string Plan { get; set; }
        public decimal CostByDay { get; set; }
        public float FineAmount { get; set; } = 0;
        public int Days { get; set; }
    }
}
