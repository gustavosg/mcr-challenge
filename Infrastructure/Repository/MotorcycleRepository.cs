using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MotorcycleRepository : Repository<MotorcycleModel, Guid>, IMotorcycleRepository
    {
        private readonly AppDbContext dbContext;
        public MotorcycleRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<MotorcycleModel>> FilterAsync(Guid? id, int? year, string plate = "", string model = "")
        {
            IQueryable<MotorcycleModel> data = from motorcycle in this.dbContext.Set<MotorcycleModel>()
                                               where !id.HasValue || id.Equals(motorcycle.Id)
                                               where !year.HasValue || year.Equals(motorcycle.Year)
                                               where String.IsNullOrWhiteSpace(plate) || plate.Equals(motorcycle.Plate)
                                               where String.IsNullOrWhiteSpace(model) || model.Equals(motorcycle.Model)
                                               orderby motorcycle.Plate ascending
                                               select motorcycle;

            return await data.ToListAsync();
        }
    }
}
