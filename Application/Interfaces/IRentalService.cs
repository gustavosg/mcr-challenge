using Application.DTO.Application.Rental;
using Core.Response;

namespace Application.Interfaces
{
    public interface IRentalService
    {
        Task<UseCaseResponse<RentalGetResponseDTO>> GetAsync(Guid id);
        Task<UseCaseResponse<List<RentalGetResponseDTO>>> GetListAsync(RentalGetRequestDTO filter);
    }
}
