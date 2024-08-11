using Models;
using Microsoft.EntityFrameworkCore;
using DTOs.Common;
using Models.Entities.Shared;

namespace DataAccess.IRepository
{
    public interface IResourcesRepository : IGenericRepository<Resources>
    {
        Task<List<LookUpDTO>> getLangObject(Guid langID, int ModuleID);
    }
}
