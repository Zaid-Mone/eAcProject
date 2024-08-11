using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Auth;
using Models.Entities.Shared;

namespace Models.Entities
{
    public class TechnicianCategory : BaseEntity
    {
        [ForeignKey("TechnicianID")]
        public long TechnicianID { get; set; }
        public Technician Technician { get; set; } = null!;



        [ForeignKey("ServiceCategoryID")]
        public long ServiceCategoryID { get; set; }
        public ServiceCategory ServiceCategory { get; set; } = null!;
    }

}
