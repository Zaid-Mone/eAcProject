using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Products
{
    public class ProductDTO
    {
        public string EnglishProductName { get; set; }
        public string ArabicProductName { get; set; }
        public string EnglishDescription { get; set; }
        public string ArabicDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid CategoryID { get; set; }
        public Guid ProviderID { get; set; }
    }
    public class ProductImageDTO
    {
        [Required]
        public Guid ProductID { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }

    }
    public class UserProductDTO
    {
        //public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EnglishUserName { get; set; }
        //public string PhoneNumber { get; set; }
        public string UserID { get; set; }
        public string Avatar { get; set; }
        public string EnglishProductName { get; set; }
        public string EnglishDescription { get; set; }
        public string ProductID { get; set; }
        public string ProviderID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string EnglishTitle { get; set; }
        public string CategoryID { get; set; }
        public List<string> ProductImages { get; set; }
    }
}
