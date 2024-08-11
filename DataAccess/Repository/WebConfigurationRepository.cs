using DataAccess.Context;
using DataAccess.IRepository;
using Models.Entities.Shared;

namespace DataAccess.Repository
{
    public class WebConfigurationRepository : GenericRepository<WebConfiguration>, IWebConfigurationRepository
    {
        public WebConfigurationRepository(eShopContext context) : base(context) { }

        public string GetValueByKeyName(string key)
        {
            var value = this.FindByCondition(q => q.ConfigKey.Equals(key, StringComparison.OrdinalIgnoreCase)).ConfigValue;
            return (!string.IsNullOrEmpty(value) ? value : "");
        }
    }

}
