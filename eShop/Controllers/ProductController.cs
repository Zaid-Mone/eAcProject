using Common.Cloudinary;
using DataAccess.Helper;
using DataAccess.IRepository;
using DTOs.Auth;
using DTOs.Enums;
using DTOs.Products;
using eShop.CustomAttributes;
using eShop.Middleware;
using eShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System.Text.Json;

namespace eShop.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomAuth(groupIds: [(int)UserGroups.Provider])]
    public class ProductController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IProductRepository _ProductRepository;
        private readonly IProductImageRepository _ProductImageRepository;
        private readonly IProviderRepository _ProviderRepository;
        private readonly ILoggerHelper _loggerHelper;
        private readonly IProductProviderRepository _ProductProviderRepository;
        public ProductController(ICloudinaryService cloudinaryService,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IProviderRepository providerRepository,
            IProductProviderRepository productProviderRepository,
            ILoggerHelper loggerHelper)
        {
            this._cloudinaryService = cloudinaryService;
            _ProductRepository = productRepository;
            _ProductImageRepository = productImageRepository;
            _ProviderRepository = providerRepository;
            _ProductProviderRepository = productProviderRepository;
            _loggerHelper = loggerHelper;
        }

        //[HttpPost("AddProduct")]
        [HttpPost]
        public ActionResult AddProduct(ProductDTO DTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var check = _ProductRepository.FindIsExistByCondition(x => x.EnglishProductName == DTO.EnglishProductName);
                    if (check)
                    {
                        return BadRequest("The Product Is Exist");
                    }

                    var product = _ProductRepository.Insert(new Product
                    {
                        ArabicProductName = DTO.EnglishProductName,
                        EnglishProductName = DTO.EnglishProductName,
                        ArabicDescription = DTO.EnglishDescription,
                        EnglishDescription = DTO.EnglishDescription,
                        CategoryID = DTO.CategoryID,
                        Price = DTO.Price,
                        Quantity = DTO.Quantity,
                        IsDeleted = false,
                        CreationDate = DateTime.Now,
                    });
                    _ProductRepository.Commit();
                    //add Product Provider
                    _ProductProviderRepository.Insert(new ProductProvider
                    {
                        ProductID = product.ID,
                        IsDeleted = false,
                        ProviderID = DTO.ProviderID
                    });
                    _ProductProviderRepository.Commit();
                    _loggerHelper.AddLog(HttpContext, DTO);
                    return Ok("Add Product has been added Successfully");
                }
                else
                {
                    return BadRequest("Something went wrong.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong.");
            }
        }

        //[HttpPost("AddProductImage")]
        [HttpPost]
        public async Task<ActionResult> AddProductImage([FromForm] ProductImageDTO model)
        {
            try
            {
                // for test
                //var headerTokenAuth = Request.Headers["Authorization"];

                var forms = Request.Form["products"].ToString();
                var desserlize = JsonSerializer.Deserialize<ProductImageDTO>(forms);
                var imageLink = string.Empty;
                var file = HttpContext.Request.Form.Files;
                if (file.Count() > 0)
                {
                    imageLink = await _cloudinaryService.SaveImage(file);
                }
                if (string.IsNullOrEmpty(imageLink) || !imageLink.Contains("cloudinary.com"))
                {
                    return BadRequest(imageLink);
                }
                var productImages = _ProductImageRepository.Insert(new ProductImages
                {
                    ProductID = model.ProductID,
                    Image = imageLink,
                    IsDeleted = false,
                });
                _ProductImageRepository.Commit();
                await _loggerHelper.AddLog(HttpContext, desserlize);
                return Ok("You have uploaded file successfully.");



            }
            catch (Exception)
            {
                return BadRequest("Something went wrong.");
            }
        }

        //[HttpGet("GetProductsByProviderId")]
        [HttpGet]
        public async Task<ActionResult> GetProductsByProviderId(string ProviderID = "")
        {
            try
            {
                Guid providerID;
                bool isValidproviderID = Guid.TryParse(ProviderID, out providerID);
                providerID = isValidproviderID ? providerID : Guid.Empty;
                var products = await _ProductRepository.getProviderProducts(providerID);
                await _loggerHelper.AddLog(HttpContext);
                return Ok(products);
            }
            catch (Exception)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest("Something went wrong.");
            }
        }

    }
}
