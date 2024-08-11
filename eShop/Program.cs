using eShop.Middleware;
using eShop.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Configuration.AddJsonFile("eProjectSettings.json", false, true);

builder.Services.AddControllers();
builder.Services.RegisterServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{

    if (app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
    }
    else
    {
        // To deploy on IIS
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
    }

});

app.UseMiddleware<TokenMiddleware>();
app.UseMiddleware<ApiWrapper>();
app.UseHttpsRedirection();

app.UseCors("CoresPolicy");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
