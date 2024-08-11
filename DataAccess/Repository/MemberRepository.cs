using DataAccess.Context;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Models.Entities;

namespace DataAccess.Repository
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MemberRepository(eShopContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
