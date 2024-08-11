using Models.Entities.Shared;

namespace Models.Entities.Auth
{
    public class UserRole : BaseEntity
    {
        public long RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public long UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
