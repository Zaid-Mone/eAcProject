using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace eShop.CustomAttributes
{
    public class CustomAuthAttribute : Attribute, IAsyncActionFilter
    {
        private int[] GroupIds;
        public CustomAuthAttribute(int[] groupIds)
        {
            GroupIds = groupIds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var token = context.HttpContext.Items["Token"] as string;
                if (string.IsNullOrEmpty(token))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    await context.HttpContext.Response.WriteAsync("UnAuthorized");
                }
                else
                {
                    var handler = new JwtSecurityTokenHandler();
                    var JWT = handler.ReadJwtToken(token);
                    if (!GetValidDate(JWT))
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        await context.HttpContext.Response.WriteAsync("UnAuthorized");
                    }
                    else
                    {
                        var GroupID = Convert.ToInt32(JWT.Claims.FirstOrDefault(x => x.Type == "GroupID").Value);

                        if (!GroupIds.Contains(GroupID))
                        {
                            context.HttpContext.Response.StatusCode = 403;
                            await context.HttpContext.Response.WriteAsync("Access denied");
                        }
                        else
                        {
                            await next();
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private bool GetValidDate(JwtSecurityToken jwt)
        {
            try
            {
                var exp = jwt.Payload.Exp.Value;
                var expDate = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime;
                if (expDate > DateTime.UtcNow)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}


