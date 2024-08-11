using DataAccess.Context;
using DataAccess.IRepository;
using Models.Entities;

namespace DataAccess.Repository
{
    public class ProductProviderRepository : GenericRepository<ProductProvider>, IProductProviderRepository
    {
        public ProductProviderRepository(eShopContext context) : base(context) { }



    }

}
