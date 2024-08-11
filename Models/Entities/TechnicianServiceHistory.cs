using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Auth;
using Models.Entities.Shared;

namespace Models.Entities
{
    public class TechnicianServiceHistory : BaseEntity
    {
        [ForeignKey("TechnicianID")]
        public long TechnicianID { get; set; }
        public Technician Technician { get; set; } = null!;

        [ForeignKey("ReservationID")]
        public long ReservationID { get; set; }
        public Reservation Reservation { get; set; } = null!;

        public DateTime ServiceDate { get; set; } // Date when the service was performed
        public string ServiceDetails { get; set; } = string.Empty; // Details about the service performed
    }

}
