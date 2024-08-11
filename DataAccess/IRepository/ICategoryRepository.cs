using Models.Entities;
using Models;

namespace DataAccess.IRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public List<Category> Search(string filter = "");
        public int Count();
        public void CategorySeeding();

    }
}
