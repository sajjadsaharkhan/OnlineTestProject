using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineTest.Bootstrapper.Jwt.Infra;
using Project.Model.DataModels;

namespace Project.Bootstrapper.Jwt
{
    public class JwtHandel : IJwtHandel
    {
        public TokenModel GenerateToken(User user)
        {
            var claims = getClaims(user);

            var securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes("3G@@mJWTSecret1234567"));
            var signin = new SigningCredentials
                (securityKey, SecurityAlgorithms.HmacSha256Signature);

            var descriptor = new SecurityTokenDescriptor()
            {
                Audience = "3Gaam",
                Issuer = "3Gaam",
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(7),
                NotBefore = DateTime.Now,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signin,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return new TokenModel(user.Firstname,
                                    user.Lastname,
                                    token,
                                    descriptor.Expires.GetValueOrDefault(),
                                    user.Username);
        }

        private List<Claim> getClaims(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));
            claims.Add(new Claim(ClaimTypes.Name, user.Firstname));
            claims.Add(new Claim(ClaimTypes.Surname, user.Lastname));
            claims.Add(new Claim(ClaimTypes.SerialNumber, user.Stamp.ToString()));
            return claims;
        }
    }
}
