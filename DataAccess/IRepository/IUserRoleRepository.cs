using Models;
using Models.Entities.Auth;

namespace DataAccess.IRepository
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        public int Count();
        public string GetUserRole(Guid userID);
    }
}
