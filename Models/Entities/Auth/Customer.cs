using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Shared;

namespace Models.Entities.Auth
{
    public class Customer : BaseEntity
    {
        [ForeignKey("UserID")]
        public long UserID { get; set; }
        public User User { get; set; }
    }
}
