using Application.DTO.Application;
using Application.DTO.Application.Motorcycle;
using Core.Response;

namespace Application.Interfaces
{
    public interface IMotorcycleService
    {
        Task<UseCaseResponse<MotorcycleAddResponseDTO>> AddAsync(MotorcycleAddRequestDTO model);
        Task<UseCaseResponse<GenericDeleteResponseDTO>> DeleteAsync(Guid id);
        Task<UseCaseResponse<MotorcycleEditResponseDTO>> EditAsync(Guid id, MotorcycleEditRequestDTO model);
        Task<UseCaseResponse<MotorcycleGetResponseDTO>> GetAsync(Guid id);
        Task<UseCaseResponse<List<MotorcycleGetResponseDTO>>> GetListAsync(MotorcycleGetRequestDTO filter);
    }
}
