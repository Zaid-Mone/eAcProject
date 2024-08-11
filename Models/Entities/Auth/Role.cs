using Models.Entities.Shared;

namespace Models.Entities.Auth
{
    public class Role : BaseEntity
    {
        public string EnglishRoleName { get; set; }
        public string ArabicRoleName { get; set; }
    }
}
