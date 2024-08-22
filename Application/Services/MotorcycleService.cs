using AutoMapper;
using FluentValidation.Results;

using Application.DTO.Application;
using Application.DTO.Application.Motorcycle;
using Application.Interfaces;
using Application.Validations;
using Core.Entities;
using Core.Response;
using Infrastructure.UnitOfWork;

namespace Application.Services
{
    public class MotorcycleService(
        IMapper mapper, 
        IMotorcycleAdapterService motorcycleAdapterService,
        IUnitOfWork unitOfWork) : IMotorcycleService
    {

        private readonly IMapper mapper = mapper;
        private readonly IMotorcycleAdapterService motorcycleAdapterService = motorcycleAdapterService;
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly MotorcycleAddRequestValidation addRequestValidation = new();
        private readonly MotorcycleEditRequestValidation editRequestValidation = new();

        public async Task<UseCaseResponse<MotorcycleAddResponseDTO>> AddAsync(MotorcycleAddRequestDTO model)
        {
            UseCaseResponse<MotorcycleAddResponseDTO> useCaseResponse = new();
            ValidationResult validationResult = await this.addRequestValidation.ValidateAsync(model);

            if (!validationResult.IsValid)
                return useCaseResponse.BadRequest($"Error occurred in the request: {string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))}");

            string check = await CheckMotorcycleAlreadyExists(model.Plate);

            if (!String.IsNullOrWhiteSpace(check))
                return useCaseResponse.BadRequest(check);

            MotorcycleModel entity = this.mapper.Map<MotorcycleModel>(model);

            await this.motorcycleAdapterService.SendAsync(model);

            return new UseCaseResponse<MotorcycleAddResponseDTO>().Ok(null);
        }

        public async Task<UseCaseResponse<GenericDeleteResponseDTO>> DeleteAsync(Guid id)
        {
            UseCaseResponse<GenericDeleteResponseDTO> useCaseResponse = new();
            MotorcycleModel entity = await this.unitOfWork.Motorcycle.Get(id);

            if ((await this.unitOfWork.MotorcycleRental.Get(x => x.MotorcycleId.Equals(id) && x.IsActive)).FirstOrDefault() is MotorcycleRentalModel found)
                return useCaseResponse.BadRequest($"It's not possible to remove Motorcycle {entity.Plate} because it has a Rental active. Begin: {found.DateBegin} - End: {found.DateEnd}");

            List<Guid> rentalIds = (await this.unitOfWork.MotorcycleRental.Get(x => x.MotorcycleId.Equals(id))).Select(x => x.Id).ToList();

            await this.unitOfWork.MotorcycleRental.DeleteByExpression(x => rentalIds.Contains(x.Id));
            await this.unitOfWork.Motorcycle.DeleteById(id);
            await this.unitOfWork.CommitAsync();

            return useCaseResponse.Ok(null!);
        }


        public async Task<UseCaseResponse<MotorcycleEditResponseDTO>> EditAsync(Guid id, MotorcycleEditRequestDTO model)
        {
            UseCaseResponse<MotorcycleEditResponseDTO> useCaseResponse = new();
            ValidationResult validationResult = await this.editRequestValidation.ValidateAsync(model);

            if (!validationResult.IsValid)
                useCaseResponse.BadRequest($"Error occurred in the request: {string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage))}");

            string check = await CheckMotorcycleAlreadyExists(model.Plate, id);

            if (!String.IsNullOrWhiteSpace(check))
                return useCaseResponse.BadRequest(check);

            MotorcycleModel entity = await this.unitOfWork.Motorcycle.Get(id);

            this.mapper.Map<MotorcycleEditRequestDTO, MotorcycleModel>(model, entity);

            this.unitOfWork.Motorcycle.Update(entity);
            await this.unitOfWork.CommitAsync();

            return useCaseResponse.Ok(this.mapper.Map<MotorcycleEditResponseDTO>(entity));
        }

        public async Task<UseCaseResponse<MotorcycleGetResponseDTO>> GetAsync(Guid id)
        {
            UseCaseResponse<MotorcycleGetResponseDTO> useCaseResponse = new();
            MotorcycleModel motorcycle = await this.unitOfWork.Motorcycle.Get(id);

            if (motorcycle is null)
                return useCaseResponse.NotFound($"Motorcycle with id {id} not found");

            return useCaseResponse.Ok(this.mapper.Map<MotorcycleGetResponseDTO>(motorcycle));
        }

        public async Task<UseCaseResponse<List<MotorcycleGetResponseDTO>>> GetListAsync(MotorcycleGetRequestDTO filter)
        {
            UseCaseResponse<List<MotorcycleGetResponseDTO>> useCaseResponse = new();
            List<MotorcycleModel> motorcycles = await this.unitOfWork.Motorcycle.FilterAsync(filter.Id, filter.Year, filter.Plate, filter.Model);

            return useCaseResponse.Ok(this.mapper.Map<List<MotorcycleGetResponseDTO>>(motorcycles));
        }

        private async Task<string> CheckMotorcycleAlreadyExists(string plate, Guid? id = null)
        {
            MotorcycleModel exists = null;

            if (id is Guid)
                exists = (await this.unitOfWork.Motorcycle.Get(x => x.Id.Equals(id.Value) && x.Plate.Equals(plate))).FirstOrDefault();
            else
                exists = (await this.unitOfWork.Motorcycle.Get(x => x.Plate.Equals(plate))).FirstOrDefault();

            if (exists is not null)
                return ("Plate already exists");

            return String.Empty;

        }
    }
}

