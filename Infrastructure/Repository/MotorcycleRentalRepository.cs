using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MotorcycleRentalRepository : Repository<MotorcycleRentalModel, Guid>, IMotorcycleRentalRepository
    {
        private readonly AppDbContext dbContext;
        public MotorcycleRentalRepository(AppDbContext dbContext): base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<MotorcycleRentalModel>> FilterAsync(Guid? id, Guid? rentalId, Guid? motorcycleId, Guid? deliveryPersonId, DateOnly? dateBegin, DateOnly? dateEnd, DateOnly? expectedDateEnd)
        {
            IQueryable<MotorcycleRentalModel> query = from mr in this.dbContext.Set<MotorcycleRentalModel>()
                                                      where !id.HasValue || id.Equals(mr.Id)
                                                      where !rentalId.HasValue || rentalId.Equals(mr.RentalId)
                                                      where !motorcycleId.HasValue || motorcycleId.Equals(mr.MotorcycleId)
                                                      where !deliveryPersonId.HasValue || deliveryPersonId.Equals(mr.DeliveryPersonId)
                                                      where !dateBegin.HasValue || dateBegin.Equals(mr.DateBegin)
                                                      where !dateEnd.HasValue || dateEnd.Equals(mr.DateEnd)
                                                      where !expectedDateEnd.HasValue || expectedDateEnd.Equals(mr.ExpectedDateEnd)
                                                      select mr;

            return await query.ToListAsync();
        }
    }
}
