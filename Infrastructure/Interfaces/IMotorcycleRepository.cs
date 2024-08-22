using Core.Entities;


namespace Infrastructure.Interfaces
{
    public interface IMotorcycleRepository : IRepository<MotorcycleModel, Guid>
    {
        Task<List<MotorcycleModel>> FilterAsync(Guid? id, int? year, string plate = "", string model = "");
    }
}
