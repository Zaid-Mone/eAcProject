namespace Models.Entities.Shared
{
    public class Language : BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string TagName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
