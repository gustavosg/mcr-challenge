using Core.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<UserModel, Guid>
    {
    }
}
