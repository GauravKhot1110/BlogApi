using BlogAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace BlogAPI.Helper
{
    public class JWTHelper
    {
        private readonly IConfiguration _config;
        public JWTHelper(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public string GenerateToken(UserLogin userLogin)
        {
            string tokenstring = string.Empty;
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,userLogin.Email),
                    //new Claim(ClaimTypes.Role,userLogin.Role)
                };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    //claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
