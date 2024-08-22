using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Application.MotorcycleRental
{
    public class MotorcycleRentalGetRequestDTO
    {
        public Guid? Id { get; set; }
        public Guid? DeliveryPersonId { get; set; }
        public Guid? RentalId { get; set; }
        public Guid? MotorcycleId { get; set; }
        public DateOnly? DateBegin { get; set; }
        public DateOnly? DateEnd { get; set; }
        public DateOnly? ExpectedDateEnd { get; set; }
    }
}
