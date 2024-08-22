using Core.Entities;

namespace Infrastructure.Interfaces
{
    public interface IRentalRepository : IRepository<RentalModel, Guid>
    {
        Task<List<RentalModel>> FilterAsync(Guid? id, string plan, decimal? costByDay, int? days);
    }
}
