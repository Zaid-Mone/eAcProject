using DataAccess.Context;
using DataAccess.IRepository;
using Models.Entities.Auth;

namespace DataAccess.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly eShopContext _context;
        public RoleRepository(eShopContext context) : base(context) { _context = context; }

        public Guid GetRoleIdByName(string roleName)
        {
            //var roleID = _context.tblRoles.Where(q => q.IsDeleted == false && q.EnglishRoleName.ToLower().Contains(roleName.ToLower()))
            //    .Select(q => q.ID)
            //    .FirstOrDefault();
            var roleID = _context.tblRoles
                    .Where(q => !q.IsDeleted && q.EnglishRoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                    .Select(q => q.ID)
                    .FirstOrDefault();
            return roleID.Equals(Guid.Empty) ? Guid.Empty : roleID;
        }

        public void RoleSeeding()
        {
            List<Role> roles = new List<Role>()
            {
                new Role(){ArabicRoleName="SuperAdmin",EnglishRoleName="SuperAdmin",IsDeleted=false},
                new Role(){ArabicRoleName="Admin",EnglishRoleName="Admin",IsDeleted=false},
                new Role(){ArabicRoleName="Member",EnglishRoleName="Member",IsDeleted=false},
                new Role(){ArabicRoleName="Provider",EnglishRoleName="Provider",IsDeleted=false}
            };
            _context.AddRange(roles);
            _context.SaveChanges();

        }
    }

}
