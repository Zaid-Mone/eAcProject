using DataAccess.Context;
using DataAccess.IRepository;
using DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Shared;

namespace DataAccess.Repository
{
    public class ResourcesRepository : GenericRepository<Resources>, IResourcesRepository
    {
        private readonly eShopContext _context;
        public ResourcesRepository(eShopContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<LookUpDTO>> getLangObject(Guid langID, int ModuleID)
        {
            try
            {
                var Result = await (from Resources in _context.Resources
                                    join Language in _context.Languages on Resources.language.ID equals Language.ID
                                    where !Resources.IsDeleted
                                    && !Language.IsDeleted
                                    && Language.ID == langID
                                    && (ModuleID == 0 ? true : ModuleID == Resources.ModulesID)
                                    select new LookUpDTO
                                    {
                                        id = Resources.ID,
                                        key = Resources.Key,
                                        value = Resources.Value
                                    }).ToListAsync();



                return Result;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }


}
