using Core.Entities;

namespace Application.DTO.Application.Motorcycle
{
    public class MotorcycleGetResponseDTO : BaseEntity
    {
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public string FullDescription { get; set; }
    }
}
