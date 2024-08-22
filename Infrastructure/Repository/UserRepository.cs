using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : Repository<UserModel, Guid>, IUserRepository
    {
        public UserRepository(DbContext dbcontext) : base(dbcontext)
        {
        }
    }
}
