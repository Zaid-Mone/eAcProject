using Models.Entities.Auth;
using Models.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class ReservationDetails : BaseEntity
    {
        [ForeignKey("TechnicianID")]
        public long TechnicianID { get; set; }
        public Technician Technician { get; set; } = null!;

        [ForeignKey("ReservationID")]
        public long ReservationID { get; set; }
        public Reservation Reservation { get; set; } = null!;

        public DateTime ServiceDate { get; set; } // Scheduled date and time for the service
        public DateTime? ServiceEndDate { get; set; } // Optional end date and time for the service
        public string ServiceAddress { get; set; } = string.Empty; // Address where service is required
        public string ServiceDescription { get; set; } = string.Empty; // Description of the service required
    }

}
