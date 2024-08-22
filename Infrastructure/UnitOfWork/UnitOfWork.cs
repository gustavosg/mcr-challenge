using Infrastructure.Interfaces;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        private readonly AppDbContext dbContext = dbContext;

        private IMotorcycleRepository motorcycle;
        private IMotorcycleRentalRepository motorcycleRental;
        private IRentalRepository rental;
        private IUserRepository user;
        private IUserRoleRepository userRole;
        public IMotorcycleRepository Motorcycle 
            => this.motorcycle is null ? this.motorcycle = new MotorcycleRepository(this.dbContext) : this.motorcycle;
        public IMotorcycleRentalRepository MotorcycleRental 
            => this.motorcycleRental is null ? this.motorcycleRental = new MotorcycleRentalRepository(this.dbContext) : this.motorcycleRental;
        public IRentalRepository Rental 
            => this.rental is null ? this.rental = new RentalRepository(this.dbContext) : this.rental;
        public IUserRepository User 
            => this.user is null ? this.user = new UserRepository(this.dbContext) : this.user;
        public IUserRoleRepository UserRole 
            => this.userRole is null ? this.userRole = new UserRoleRepository(this.dbContext) : this.userRole;

        public async Task CommitAsync()
            => await this.dbContext.SaveChangesAsync();

        public async Task RollbackAsync()
            => await this.dbContext.DisposeAsync();
    }
}
