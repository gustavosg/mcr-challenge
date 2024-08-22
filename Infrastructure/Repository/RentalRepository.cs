using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class RentalRepository : Repository<RentalModel, Guid>, IRentalRepository
    {
        private readonly AppDbContext dbContext;
        public RentalRepository(AppDbContext dbContext): base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<RentalModel>> FilterAsync(Guid? id, string plan, decimal? costByDay, int? days)
        {
            IQueryable<RentalModel> query = from rent in this.dbContext.Set<RentalModel>()

                                            where !id.HasValue || id.Equals(rent.Id)
                                            where String.IsNullOrWhiteSpace(plan) || plan.Equals(rent.Plan)
                                            where !costByDay.HasValue || costByDay.Equals(rent.CostByDay)
                                            where !days.HasValue || days.Equals(rent.Days)
                                            orderby rent.Plan ascending
                                            select rent;

            return await query.ToListAsync();
        }
    }
}
