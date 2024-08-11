using DTOs.Enums;
using Models.Entities.Auth;
using Models.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class Feedback : BaseEntity
    {
        [ForeignKey("ReservationDetailsID")]
        public long ReservationDetailsID { get; set; }
        public ReservationDetails ReservationDetails { get; set; } = null!;


        // you can un commit this if you want to allow the Technician to add Feedback for the customer.
        //[ForeignKey("TechnicianID")]
        //public long? TechnicianID { get; set; } // Make optional if not always applicable
        //public Technician? Technician { get; set; }

        [ForeignKey("CustomerID")]
        public long CustomerID { get; set; }
        public Customer Customer { get; set; } = null!;

        public string Comments { get; set; } = string.Empty;

        private int _rating;
        public int Rating
        {
            get => _rating;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Rating), "Rating must be between 1 and 5.");
                _rating = value;
                Severity = CalculateSeverity(value);
            }
        }

        public DateTime FeedbackDate { get; set; }

     //   public FeedbackStatus Status { get; set; } = FeedbackStatus.Pending;

        public FeedbackSeverity Severity { get; private set; } = FeedbackSeverity.Medium; // Default severity

        private FeedbackSeverity CalculateSeverity(int rating)
        {
            if (rating >= 4)
                return FeedbackSeverity.High;
            else if (rating == 3)
                return FeedbackSeverity.Medium;
            else
                return FeedbackSeverity.Low;
        }
    }



}
