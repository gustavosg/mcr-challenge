using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.Rental
{
    public class RentalGetResponseDTO : BaseEntity
    {
        public string Plan { get; set; }
        public decimal CostByDay { get; set; }
        public float FineAmount { get; set; }
        public int Days { get; set; }
    }
}
