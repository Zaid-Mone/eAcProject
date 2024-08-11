using DataAccess.Context;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Models.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminRepository(eShopContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentLoggedInUserEmail()
        {
            var userEmailClaim = _httpContextAccessor.HttpContext.User.FindFirst("Email");
            if (userEmailClaim is not null)
            {
                return userEmailClaim.Value;
            }
            throw new InvalidOperationException("No logged-in user found.");
        }

        public Guid GetCurrentLoggedInUserID()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id");
            // Get admin ID
            var admin = this.FindByCondition(x => x.UserID == Guid.Parse(userIdClaim.Value));
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return admin.ID;
            }
            throw new InvalidOperationException("No logged-in user found.");
        }

        public string GetCurrentLoggedInUserRole()
        {
            var userRoleClaim = _httpContextAccessor.HttpContext.User.FindFirst("Role");

            if (userRoleClaim is not null)
            {
                return userRoleClaim.Value;
            }
            throw new InvalidOperationException("No logged-in user found.");
        }

    }

}
