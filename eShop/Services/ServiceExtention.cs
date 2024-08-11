using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System;
using DataAccess.Context;
using Common.JWT;
using DataAccess.IRepository;
using DataAccess.Repository;
using CloudinaryDotNet;
using Common.Cloudinary;
using DataAccess.Helper;

namespace eShop.Services
{
    public static class ServiceExtention
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Add Cors
            services.AddCors(o =>
            {
                o.AddPolicy("CoresPolicy", build =>
                {
                    build.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            // For Fluent Validation 
            //services.AddValidatorsFromAssemblyContaining<UserValidator>();
            //services.AddValidatorsFromAssemblyContaining<RoleValidator>();
            //services.AddScoped(typeof(IValidator<User>), typeof(UserValidator));
            //services.AddScoped(typeof(IValidator<Role>), typeof(RoleValidator));
            //services.AddScoped(typeof(IEmailSender), typeof(EmailSender));

            // for API Versions
            //services.AddApiVersioning(opt =>
            //{
            //    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            //    opt.AssumeDefaultVersionWhenUnspecified = true;
            //    opt.ReportApiVersions = true;
            //    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
            //                                                    new HeaderApiVersionReader("x-api-version"),
            //                                                    new MediaTypeApiVersionReader("x-api-version"));
            //});

            services.AddSingleton<Cloudinary>();
            services.AddScoped<Cloudinary>((sp) =>
            {
                Account account = new Account(
                        configuration.GetSection("CloudName").Value,
                        configuration.GetSection("CloudApiKey").Value,
                        configuration.GetSection("CloudApiSecret").Value
                    );
                return new Cloudinary(account);
            });
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            //services.AddSingleton(typeof(IWebConfigurationRepository), typeof(WebConfigurationRepository));
            services.AddDbContext<eShopContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("eShopDb"), b => b.MigrationsAssembly(typeof(eShopContext).Assembly.FullName));
            });




            // Jwt Bearer 
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        opt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidateLifetime = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("secret_Key").Value)),
            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var secretKey = configuration.GetSection("secret_Key").Value;
                    if (string.IsNullOrEmpty(secretKey))
                    {
                        throw new ArgumentNullException("secret_Key", "The secret key is missing in configuration.");
                    }

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Repositories Services.
            services.AddScoped(typeof(ITokenGenerator), typeof(TokenGenerator));
            services.AddScoped(typeof(IAdminRepository), typeof(AdminRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IProviderRepository), typeof(ProviderRepository));
            services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            services.AddScoped(typeof(IUserRoleRepository), typeof(UserRoleRepository));
            services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
            services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            services.AddScoped(typeof(IProductImageRepository), typeof(ProductImageRepository));
            services.AddScoped(typeof(IProductProviderRepository), typeof(ProductProviderRepository));
            services.AddScoped(typeof(IWebConfigurationRepository), typeof(WebConfigurationRepository));
            services.AddScoped(typeof(ILoggerHelper), typeof(LoggerHelper));
            services.AddScoped(typeof(IResourcesRepository), typeof(ResourcesRepository));
            services.AddScoped(typeof(ILanguageRepository), typeof(LanguageRepository));
            services.AddScoped(typeof(IMemberRepository), typeof(MemberRepository));




            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                // download this package for it  Swashbuckle.AspNetCore.Annotations
                setup.EnableAnnotations();

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

            });
            services.AddHttpContextAccessor();
        }
    }
}

