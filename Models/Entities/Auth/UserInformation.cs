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
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime BirthDate { get; set; }
        public string PostalCode { get; set; }
        public Gender Gender { get; set; }
        public long AreasID { get; set; }
        public Areas Areas { get; set; }
        public long CreationBy { get; set; }
        public DateTime CreationDate { get; set; }
        public long ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }



        // Optional: Latitude and Longitude for more precise location
        //public decimal? Latitude { get; set; }
        //public decimal? Longitude { get; set; }
    }
    public class Countries : BaseEntity
    {
        public string NameEN { get; set; }
        public string NameAR { get; set; }
        public string Counrty_Code { get; set; }
        
        public long CreationBy { get; set; }
        public DateTime CreationDate { get; set; }
        public long ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
    }
    public class Cities : BaseEntity
    {
        public string NameEN { get; set; }
        public string NameAR { get; set; }
        public long CountryID { get; set; }
        public Country Country { get; set; }
        
        public long CreationBy { get; set; }
        public DateTime CreationDate { get; set; }
        public long ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
    }    
    public class Areas : BaseEntity
    {
        public string NameEN { get; set; }
        public string NameAR { get; set; }
        public long CitiesID { get; set; }
        public Cities Cities { get; set; }
        
        public long CreationBy { get; set; }
        public DateTime CreationDate { get; set; }
        public long ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
