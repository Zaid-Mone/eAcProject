using Models;
using Models.Entities.Auth;

namespace DataAccess.IRepository
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public void RoleSeeding();
        public Guid GetRoleIdByName(string roleName);
    }
}
