using AutoMapper;

using Application.DTO.Application.Rental;
using Application.Interfaces;
using Core.Entities;
using Core.Response;
using Infrastructure.UnitOfWork;

namespace Application.Services
{
    public class RentalService(IMapper mapper, IUnitOfWork unitOfWork) : IRentalService
    {
        private readonly IMapper mapper = mapper;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<UseCaseResponse<RentalGetResponseDTO>> GetAsync(Guid id)
        {
            UseCaseResponse<RentalGetResponseDTO> useCaseResponse = new();

            RentalModel entity = await this.unitOfWork.Rental.Get(id);

            if (entity is null) 
                return useCaseResponse.NotFound($"Rental with id {id} not found");

            return useCaseResponse.Ok(this.mapper.Map<RentalGetResponseDTO>(entity));
        }

        public async Task<UseCaseResponse<List<RentalGetResponseDTO>>> GetListAsync(RentalGetRequestDTO filter)
        {
            UseCaseResponse<List<RentalGetResponseDTO>> useCaseResponse = new();

            List<RentalModel> data = await this.unitOfWork.Rental.FilterAsync(filter.Id, filter.Plan, filter.CostByDay, filter.Days);
            
            if (!data.Any())
                return useCaseResponse.NotFound($"No data found in Rentals");

            return useCaseResponse.Ok(this.mapper.Map<List<RentalGetResponseDTO>>(data));
        }
    }
}
