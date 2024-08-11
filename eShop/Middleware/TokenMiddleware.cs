using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;

namespace eShop.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                string token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt  = handler.ReadJwtToken(token);
                    httpContext.Items["Token"] = token;
                    string userId = Convert.ToString(jwt.Claims.FirstOrDefault(x => x.Type == "UserID").Value);
                    httpContext.Items["UserId"] = userId;
                    int groupId = Convert.ToInt32(jwt.Claims.FirstOrDefault(x => x.Type == "GroupID").Value);
                    httpContext.Items["GroupID"] = groupId;
                }
                else
                {
                    httpContext.Items["Token"] = "";

                }
                   await _next(httpContext);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }

}
