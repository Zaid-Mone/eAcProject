using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Auth;
using Models.Entities.Shared;

namespace Models.Entities
{
    public class Product : BaseEntity
    {
        public string EnglishProductName { get; set; }
        public string ArabicProductName { get; set; }
        public string EnglishDescription { get; set; }
        public string ArabicDescription { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("AdminID")]
        public long AddedBy { get; set; }
        public Admin Admin { get; set; }
        //[NotMapped]
        //public List<ProductImages> ProductImages { get; set; }

    }
}
