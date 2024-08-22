using Infrastructure.Interfaces;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IMotorcycleRepository Motorcycle { get; }
        public IMotorcycleRentalRepository MotorcycleRental { get; }
        public IRentalRepository Rental { get; }
        public IUserRepository User { get; }
        public IUserRoleRepository UserRole { get; }

        Task CommitAsync();
        Task RollbackAsync();
    }
}
