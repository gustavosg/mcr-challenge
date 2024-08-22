using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.Rental
{
    public class RentalGetRequestDTO
    {
        public Guid? Id { get; set; }
        public string? Plan { get; set; }
        public decimal? CostByDay { get; set; }
        public int? Days { get; set; }

    }
}
