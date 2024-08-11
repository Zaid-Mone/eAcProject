using DataAccess.Context;
using DataAccess.IRepository;
using Models.Entities.Auth;

namespace DataAccess.Repository
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(eShopContext context) : base(context) { }

        public int Count()
        {
            return this.FindAllByCondition(q => q.IsDeleted == false).Count();
        }

        public string GetUserRole(Guid userID)
        {
            return this.FindByConditionWithIncludes(
                ur => ur.UserId == userID,
                u => u.Role)?.Role?.EnglishRoleName;
        }
    }

}
