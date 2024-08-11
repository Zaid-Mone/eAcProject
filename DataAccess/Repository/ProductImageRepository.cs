using DataAccess.Context;
using DataAccess.IRepository;
using Models.Entities;

namespace DataAccess.Repository
{
    public class ProductImageRepository : GenericRepository<ProductImages>, IProductImageRepository
    {
        public ProductImageRepository(eShopContext context) : base(context) { }

    }

}
