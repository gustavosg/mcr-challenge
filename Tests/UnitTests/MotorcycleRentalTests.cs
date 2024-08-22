using Application.DTO.Application.MotorcycleRental;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Core.Entities;
using Core.Response;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Tests.Unit
{
    public class MotorcycleRentalTests
    {
        private readonly MotorcycleRentalService motorcycleRentalService;

        private readonly RentalModel rental1 = new()
        {
            Id = Guid.Parse("9e802fbe-3f87-4109-bcbb-32e0ff43f2dc"),
            CostByDay = 30,
            Days = 7,
            Plan = "7 days",
            FineAmount = 20
        };
        private readonly RentalModel rental2 = new()
        {
            Id = Guid.Parse("192916bb-eea2-4a46-8cf9-f51c1e2323a4"),
            CostByDay = 28,
            Days = 15,
            Plan = "15 days",
            FineAmount = 40
        };

        private readonly MotorcycleRentalModel motorcycleRental1 = new()
        {
            Id = Guid.Parse("d5032315-ba7b-426a-bba4-7abe3af9ea41"),
            RentalId = Guid.Parse("9e802fbe-3f87-4109-bcbb-32e0ff43f2dc"),
            MotorcycleId = Guid.Parse("f2a94b0a-6be3-413d-806a-73beeecda467"),
            DeliveryPersonId = Guid.Parse("31207eaa-a7d0-4ea4-87ba-8dc050f0527a"),
            DateBegin = DateOnly.FromDateTime(DateTime.Now),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            ExpectedDateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            EstimatedCost = 100,
            FinalCost = 100,
            IsActive = true
        };
        
        private readonly MotorcycleRentalModel motorcycleRental2 = new()
        {
            Id = Guid.Parse("769038db-dce0-4aaf-90e6-6cebaf9044ca"),
            RentalId = Guid.Parse("192916bb-eea2-4a46-8cf9-f51c1e2323a4"),
            MotorcycleId = Guid.Parse("f2a94b0a-6be3-413d-806a-73beeecda467"),
            DeliveryPersonId = Guid.Parse("31207eaa-a7d0-4ea4-87ba-8dc050f0527a"),
            DateBegin = DateOnly.FromDateTime(DateTime.Now),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            ExpectedDateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            EstimatedCost = 100,
            FinalCost = 100,
            IsActive = true
        };

        public MotorcycleRentalTests()
        {
            IHttpContextAccessor httpContextAccessor = Substitute.For<HttpContextAccessor>();
            IMapper mapper = Substitute.For<IMapper>();
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            
            unitOfWork.MotorcycleRental.Get(this.motorcycleRental1.Id).Returns(motorcycleRental1);
            unitOfWork.MotorcycleRental.Get(this.motorcycleRental2.Id).Returns(motorcycleRental2);
            unitOfWork.Rental.Get(this.rental1.Id).Returns(rental1);
            unitOfWork.Rental.Get(this.rental2.Id).Returns(rental2);

            this.motorcycleRentalService = new MotorcycleRentalService
                (httpContextAccessor, mapper, unitOfWork);

        }

        [Theory]
        [InlineData("d5032315-ba7b-426a-bba4-7abe3af9ea41", 144)]
        [InlineData("769038db-dce0-4aaf-90e6-6cebaf9044ca", 156.8)]
        public async void VerifySimulateMotorcycleReturn(Guid id, decimal expectedValue)
        {
            UseCaseResponse<MotorcycleRentalSimulatePriceResponseDTO> useCaseActual = 
                await this.motorcycleRentalService.SimulateMotorcycleReturn(id, DateOnly.FromDateTime(DateTime.Now.AddDays(4)));

            Assert.Equal(expectedValue, useCaseActual.Result.Value);
        }
    }
}