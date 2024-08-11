using Models.Entities;
using Models;
using DTOs.Products;

namespace DataAccess.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public List<Product> Search(string filter = "");
        //public int Count();
        Task<List<UserProductDTO>> getProviderProducts(Guid providerID);
    }
}
