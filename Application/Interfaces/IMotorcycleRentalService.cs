using Application.DTO.Application.MotorcycleRental;
using Core.Response;

namespace Application.Interfaces
{
    public interface IMotorcycleRentalService
    {
        Task<UseCaseResponse<MotorcycleRentalAddResponseDTO>> AddAsync(MotorcycleRentalAddRequestDTO request);
        Task<UseCaseResponse<MotorcycleRentalGetResponseDTO>> GetAsync(Guid id);
        Task<UseCaseResponse<List<MotorcycleRentalGetResponseDTO>>> GetListAsync(MotorcycleRentalGetRequestDTO request);
        Task<UseCaseResponse<MotorcycleRentalSimulatePriceResponseDTO>> SimulateMotorcycleReturn(Guid id, DateOnly dateReturn);
    }
}
