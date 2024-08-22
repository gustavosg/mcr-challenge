using Core.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserRoleRepository : IRepository<UserRoleModel, Guid>
    {
    }
}
