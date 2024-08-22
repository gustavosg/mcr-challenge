using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

using AutoMapper;

using Application.Core.Extensions;
using Application.DTO.Application.MotorcycleRental;
using Application.Interfaces;
using Application.Validations;
using Core;
using Core.Entities;
using Core.Response;
using Infrastructure.UnitOfWork;

namespace Application.Services
{
    public class MotorcycleRentalService(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUnitOfWork unitOfWork
        ) : IMotorcycleRentalService
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IMapper mapper = mapper;
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly MotorcycleRentalAddRequestValidation motorcycleAddValidation = new();


        public async Task<UseCaseResponse<MotorcycleRentalAddResponseDTO>> AddAsync(MotorcycleRentalAddRequestDTO request)
        {
            UseCaseResponse<MotorcycleRentalAddResponseDTO> useCaseResponse = new();

            ValidationResult validationResult = await this.motorcycleAddValidation.ValidateAsync(request);

            if (!validationResult.IsValid)
                return useCaseResponse.BadRequest($"Error occurred in the request: {string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))}");

            UserModel userCheck = (await this.unitOfWork.User.Get(this.httpContextAccessor.HttpContext.User.Id()));

            if (!userCheck.IsDriverCardTypeA())
                return useCaseResponse.BadRequest($"Driver not allowed to rent a Motorcycle");

            MotorcycleModel motorcycle = await this.unitOfWork.Motorcycle.Get(request.MotorcycleId);

            if (motorcycle is null)
                return useCaseResponse.NotFound($"Motorcycle with Id: {request.MotorcycleId} not found");

            RentalModel rental = await this.unitOfWork.Rental.Get(request.RentalId);

            if (rental is null)
                return useCaseResponse.NotFound($"Rental with Id: {request.RentalId} not Found");

            MotorcycleRentalModel motorcycleRentalCheck =
                (await this.unitOfWork.MotorcycleRental.Get(x =>
                    x.MotorcycleId.Equals(request.MotorcycleId)
                    && x.RentalId.Equals(request.RentalId)
                    && x.IsActive
                    )
                ).FirstOrDefault();

            if (motorcycleRentalCheck is not null)
                return useCaseResponse.BadRequest($"There is a rental active for this motorbyke");

            MotorcycleRentalModel entity = this.mapper.Map<MotorcycleRentalModel>(request);
            entity.EstimatedCost = EstimateCost(rental);
            entity.DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(rental.Days));
            entity.DeliveryPersonId = this.httpContextAccessor.HttpContext.User.Id();

            await this.unitOfWork.MotorcycleRental.Insert(entity);
            await this.unitOfWork.CommitAsync();

            return useCaseResponse.Ok(this.mapper.Map<MotorcycleRentalAddResponseDTO>(entity));
        }

        public async Task<UseCaseResponse<MotorcycleRentalGetResponseDTO>> GetAsync(Guid id)
        {
            UseCaseResponse<MotorcycleRentalGetResponseDTO> useCaseResponse = new();

            MotorcycleRentalModel entity = await this.unitOfWork.MotorcycleRental.Get(id);

            if (entity is null)
                return useCaseResponse.NotFound();

            return useCaseResponse.Ok(this.mapper.Map<MotorcycleRentalGetResponseDTO>(entity));
        }
        
        public async Task<UseCaseResponse<List<MotorcycleRentalGetResponseDTO>>> GetListAsync(MotorcycleRentalGetRequestDTO request)
        {
            UseCaseResponse<List<MotorcycleRentalGetResponseDTO>> useCaseResponse = new();

            List<MotorcycleRentalModel> entity = await this.unitOfWork.MotorcycleRental.FilterAsync(
                request.Id, 
                request.RentalId, 
                request.MotorcycleId, 
                request.DeliveryPersonId, 
                request.DateBegin, 
                request.DateEnd, 
                request.ExpectedDateEnd
            );

            if (!entity.Any())
                return useCaseResponse.NotFound();

            return useCaseResponse.Ok(this.mapper.Map<List<MotorcycleRentalGetResponseDTO>>(entity));
        }

        public async Task<UseCaseResponse<MotorcycleRentalSimulatePriceResponseDTO>> SimulateMotorcycleReturn(Guid id, DateOnly dateToSimulate)
        {
            UseCaseResponse<MotorcycleRentalSimulatePriceResponseDTO> useCaseResponse = new();

            MotorcycleRentalModel motorcycleRental = await this.unitOfWork.MotorcycleRental.Get(id);

            if (motorcycleRental is null)
                return useCaseResponse.NotFound();

            MotorcycleRentalSimulatePriceResponseDTO dto = new();

            dto.Value = await this.SimulateFinalCost(motorcycleRental.RentalId, motorcycleRental.DateBegin, motorcycleRental.ExpectedDateEnd, dateToSimulate);

            return useCaseResponse.Ok(dto);
        }

        private decimal EstimateCost(RentalModel rental)
            => rental.CostByDay * rental.Days;
        
        private async Task<decimal> SimulateFinalCost(Guid rentalId, DateOnly dateBegin, DateOnly expectedDateEnd, DateOnly simulateDateEnd)
        {
            RentalModel rental = await this.unitOfWork.Rental.Get(rentalId);

            decimal totalValue = (simulateDateEnd.DayNumber - dateBegin.DayNumber) * rental.CostByDay;

            if (simulateDateEnd.DayNumber < expectedDateEnd.DayNumber && rental.FineAmount is > 0)
                totalValue = totalValue + (totalValue * (decimal)(rental.FineAmount / 100));

            else if (simulateDateEnd.DayNumber > expectedDateEnd.DayNumber)
                totalValue = totalValue + (decimal)((simulateDateEnd.DayNumber - expectedDateEnd.DayNumber) * Constants.ADDITIONAL_VALUE_PER_DAY);

            return totalValue;
        }

    }
}
