using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRoleRepository : Repository<UserRoleModel, Guid> , IUserRoleRepository
    {
        public UserRoleRepository(DbContext dbcontext) : base(dbcontext)
        {
            
        }
    }
}
