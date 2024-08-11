using DataAccess.Context;
using DataAccess.IRepository;
using DTOs.Products;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly eShopContext _context;
        public ProductRepository(eShopContext context) : base(context) { this._context = context; }

        //public int Count()
        //{
        //    return this.FindAllByCondition(q => q.IsDeleted == false).Count();
        //}

        public async Task<List<UserProductDTO>> getProviderProducts(Guid providerID)
        {
            var result = await (from tblUser in _context.tblUsers
                                join tblPrviders in _context.tblProviders on tblUser.ID equals tblPrviders.UserID
                                join tblproductProviders in _context.tblProductProviders on tblPrviders.ID equals tblproductProviders.ProviderID
                                join tblProduct in _context.tblProducts on tblproductProviders.ProductID equals tblProduct.ID
                                join tblCategories in _context.tblCategories on tblProduct.CategoryID equals tblCategories.ID
                                where tblProduct.IsDeleted == false && tblproductProviders.ProviderID == providerID
                                select new UserProductDTO()
                                {
                                    EnglishUserName = tblUser.EnglishUserName,
                                    CategoryID = tblCategories.ID.ToString(),
                                    EnglishProductName = tblProduct.EnglishProductName,
                                    EnglishTitle = tblCategories.EnglishTitle,
                                    CreatedAt = tblProduct.CreationDate,
                                    Price = tblProduct.Price,
                                    EnglishDescription = tblProduct.EnglishDescription,
                                    ProductID = tblProduct.ID.ToString(),
                                    ProviderID = tblPrviders.ID.ToString(),
                                    Quantity = tblProduct.Quantity,
                                    UserID = tblUser.ID.ToString(),
                                    Avatar = tblUser.Avatar,
                                    ProductImages = (from tblProductImg in _context.tblProductImages
                                                     where tblProductImg.ProductID == tblProduct.ID 
                                                     select tblProductImg.Image).ToList(),

                                    
                                }).AsNoTracking().ToListAsync();
            return result;
        }

        public List<Product> Search(string filter = "")
        {
            filter = filter.ToLower();
            if (string.IsNullOrEmpty(filter))
            {
                return this.FindAllByCondition(q => q.IsDeleted == false);
            }
            else
            {
                return this.FindAllByCondition(q => q.IsDeleted == false && (q.ArabicProductName.Contains(filter)
                || q.EnglishProductName.Contains(filter)));
            }
        }
    }

}
