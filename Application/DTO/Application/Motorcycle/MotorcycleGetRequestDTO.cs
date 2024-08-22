using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.Motorcycle
{
    public class MotorcycleGetRequestDTO
    {
        public Guid? Id { get; set; }
        public string? Plate { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
    }
}
