using DataAccess.Context;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Models.Entities;

namespace DataAccess.Repository
{
    public class ProviderRepository : GenericRepository<Provider>, IProviderRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProviderRepository(eShopContext context, IHttpContextAccessor httpContextAccessor) : base(context)
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
            // Get provider ID
            var provider = this.FindByCondition(x => x.UserID == Guid.Parse(userIdClaim.Value));

            if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return provider.ID;
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
