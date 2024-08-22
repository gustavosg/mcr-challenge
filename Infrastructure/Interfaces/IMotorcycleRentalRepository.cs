using Core.Entities;

namespace Infrastructure.Interfaces
{
    public interface IMotorcycleRentalRepository : IRepository<MotorcycleRentalModel, Guid>
    {
        Task<List<MotorcycleRentalModel>> FilterAsync(Guid? id, Guid? rentalId, Guid? motorcycleId, Guid? deliveryPersonId, DateOnly? dateBegin, DateOnly? dateEnd, DateOnly? expectedDateEnd);
    }
}
