using Models;
using Models.Entities.Shared;

namespace DataAccess.IRepository
{
    public interface IWebConfigurationRepository : IGenericRepository<WebConfiguration>
    {
        string GetValueByKeyName(string key);
    }
}
