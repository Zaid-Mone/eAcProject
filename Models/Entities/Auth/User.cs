using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Shared;

namespace Models.Entities.Auth
{
    public class User : BaseEntity
    {
        public string UserName { get; set; };
        public string HashPassword { get; set; };
        public string Salt { get; set; };
        // Photo
        //public string Avatar { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // for lock out 
        public bool IsLockedOut { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime LockedOutDate { get; set; }
        //[NotMapped]
        //public ICollection<Role> Roles { get; set; } = new List<Role>();
        //[NotMapped]
        //public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
