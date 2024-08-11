using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace Models.Entities.Shared
{
    public class Resources : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int ModulesID { get; set; }
        public DateTime CreationDate { get; set; }
        public Language language { get; set; }
    }
}
