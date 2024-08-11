using Models.Entities;
using Models;

namespace DataAccess.IRepository
{
    public interface IProviderRepository : IGenericRepository<Provider>
    {
        public Guid GetCurrentLoggedInUserID();
        public string GetCurrentLoggedInUserEmail();
        public string GetCurrentLoggedInUserRole();

    }
}
