using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.MotorcycleRental
{
    public class MotorcycleRentalAddRequestDTO
    {
        public Guid MotorcycleId { get; set; }
        public Guid RentalId { get; set; }
        public DateOnly DateBegin { get; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        public DateOnly ExpectedDateEnd { get; set; }
    }
}
