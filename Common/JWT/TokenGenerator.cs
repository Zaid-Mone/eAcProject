using DataAccess.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.JWT
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        public TokenGenerator(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public string GenerateToken(string userID)
        {

            try
            {
                var userInfo = _userRepository.GetUserInfo(userID).GetAwaiter().GetResult();
                var claims = new List<Claim> {
                 new Claim("UserID",userInfo.UserID.ToString()),
                 new Claim(ClaimTypes.Role, userInfo.RoleName),
                 new Claim("Email",userInfo.Email),
                 new Claim("GroupID",userInfo.GroupID.ToString()),
                 new Claim("Admin",userInfo.AdminID),
                 new Claim("ProviderID",userInfo.ProviderID)
                 //new Claim("Role",roleuser),
                 };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("secret_Key").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };
                var tokenhandler = new JwtSecurityTokenHandler();
                var token = tokenhandler.CreateToken(tokenDescriptor);
                return tokenhandler.WriteToken(token);
            }
            catch (Exception)
            {

                return "Generated Token is Failed";
            }


        }
    }
}
