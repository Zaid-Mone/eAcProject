using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities.Auth;

namespace DataAccess.IRepository
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        public Guid GetCurrentLoggedInUserID();
        public string GetCurrentLoggedInUserEmail();
        public string GetCurrentLoggedInUserRole();

    }
}
