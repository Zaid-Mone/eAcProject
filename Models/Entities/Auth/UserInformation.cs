using DTOs.Enums;
using Models.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Auth
{
    public class UserInformation : BaseEntity
    {
        [ForeignKey("UserID")]
        public long UserID { get; set; }
        public User User { get; set; }

        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public Gender Gender { get; set; }
        //public Country Country { get; set; }



        // Optional: Latitude and Longitude for more precise location
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
