using DTOs.Enums;
using Models.Entities.Auth;
using Models.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class Reservation : BaseEntity
    {

        [ForeignKey("CustomerID")]
        public long CustomerID { get; set; } 
        public Customer Customer { get; set; } = null!;

        public DateTime ReservationDate { get; set; } // Date and time when the reservation was made ////
        public ReservationStatus Status { get; set; } // Status of the reservation (e.g., Pending, Completed, Cancelled) 


        [ForeignKey("ServiceCategoryID")]
        public long ServiceCategoryID { get; set; }
        public ServiceCategory ServiceCategory { get; set; } = null!;

    }

}
