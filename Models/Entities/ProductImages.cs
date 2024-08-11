using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Shared;

namespace Models.Entities
{
    public class ProductImages : BaseEntity
    {
        [ForeignKey("ProductID")]
        public long ProductID { get; set; }
        public Product Product { get; set; }
        public string Image { get; set; }
    }
}
