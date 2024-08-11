namespace DTOs.Users
{
    public class ProfileDTO
    {

        public int GroupID { get; set; }
        public string UserID { get; set; }
        public string UserNameEn { get; set; }
        public string UserNameAr { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Avatar { get; set; }
        public string ArabicBio { get; set; }
        public string EnglishBio { get; set; }
        public DateTime BirthDate { get; set; }
        public MemberProfileDTO MemberProfileDTO { get; set; }
        public ProviderProfileDTO ProviderProfileDTO { get; set; }
        public AdminProfileDTO AdminProfileDTO { get; set; }
    }

    public class MemberProfileDTO
    {
        public string MemberID { get; set; }
    }
    public class ProviderProfileDTO
    {
        public string ProviderID { get; set; }

    }
    public class AdminProfileDTO
    {
        public string AdminID { get; set; }
    }
}
