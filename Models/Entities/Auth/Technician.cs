using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Shared;

namespace Models.Entities.Auth
{
    public class Technician : BaseEntity // who handle repairs or maintenance of air conditioners (support / Engineer)
    {
        [ForeignKey("UserID")]
        public long UserID { get; set; }
        public User User { get; set; }
        public int YearsOfExperience { get; set; } // Number of years of experience

        public bool IsAvailable { get; set; } // Track if the technician is currently available

    }

}
