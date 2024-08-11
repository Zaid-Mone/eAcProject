using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Shared
{
    public class WebLog : BaseEntity
    {
        public string UserID { get; set; }
        public string Method { get; set; }
        public int HttpRequestType { get; set; }
        public string Params { get; set; }
        public string Token { get; set; }
        public string Controller { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
