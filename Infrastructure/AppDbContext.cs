using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : IdentityDbContext<UserModel, RoleModel, Guid>
    {
        public DbSet<DeliveryPersonModel> DeliveryPerson { get; set; }
        public DbSet<MotorcycleModel> Motorcycle { get; set; }
        public DbSet<MotorcycleRentalModel> MotorcycleRent { get; set; }
        public DbSet<RentalModel> Rental { get; set; }
        public DbSet<RoleModel> Role { get; set; }
        public DbSet<UserModel> User { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
