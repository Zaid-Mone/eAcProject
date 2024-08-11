using DataAccess.Context;
using DataAccess.IRepository;
using Models.Entities.Shared;

namespace DataAccess.Repository
{
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        private readonly eShopContext _context;
        public LanguageRepository(eShopContext context) : base(context)
        {
            _context = context;
        }

    }


}
